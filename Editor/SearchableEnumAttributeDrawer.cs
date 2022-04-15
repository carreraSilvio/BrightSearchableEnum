using BrightSearchableEnum;
using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BrightSearchableEnumEditor
{
    [CustomPropertyDrawer(typeof(SearchableEnumAttribute))]
    public class SearchableEnumAttributeDrawer : PropertyDrawer
    {
        private SerializedProperty _serializedProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _serializedProperty = property;

            var fullWidth = position.width;
            var leftWidth = fullWidth * 0.4f;
            var rightWidth = fullWidth * 0.6f;

            var fields = property.serializedObject.targetObject.GetType().GetFields();
            Type enumType = null;
            foreach (var field in fields)
            {
                if (field.Name == property.name)
                {
                    enumType = field.FieldType;
                    break;
                }

            }

            if (enumType == null)
            {
                return;
            }

            position.width = leftWidth;
            GUI.Label(position, $"{property.displayName}");
            position.x += leftWidth;
            position.width = rightWidth;
            if (GUI.Button(position, new GUIContent($"{property.enumDisplayNames[property.enumValueIndex]}"), EditorStyles.popup))
            {
                var enumSearchWindowProvider = ScriptableObject.CreateInstance<EnumSearchWindowProvider>();
                enumSearchWindowProvider.Set(enumType, HandleSelectEntry);

                SearchWindow.Open(
                    new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                    enumSearchWindowProvider);
            }
        }

        private void HandleSelectEntry(Enum obj)
        {
            _serializedProperty.enumValueIndex = Convert.ToInt32(obj);
            _serializedProperty.serializedObject.ApplyModifiedProperties();
        }
    }
}