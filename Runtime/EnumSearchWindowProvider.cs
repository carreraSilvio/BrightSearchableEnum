using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnumSearchWindowProvider : ScriptableObject, ISearchWindowProvider
{
    private Enum _enumTarget;
    private Action<Enum> _onSelectEntryCallback;
    private Type _enumType;

    public EnumSearchWindowProvider(Type enumType, Action<Enum> onSelectEntryHandler)
    {
        _enumType = enumType;
        _onSelectEntryCallback = onSelectEntryHandler;
    }

    public void Set(Type enumType, Action<Enum> onSelectEntryHandler)
    {
        _enumType = enumType;
        _onSelectEntryCallback = onSelectEntryHandler;
    }

    public void Set(Enum enumTarget, Action<Enum> onSelectEntryHandler)
    {
        _enumTarget = enumTarget;
        _onSelectEntryCallback = onSelectEntryHandler;
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        if(_enumTarget == null && _enumType == null)
        {
            return new List<SearchTreeEntry>();
        }

        var enumTypeInUse = _enumTarget != null ? _enumTarget.GetType() : _enumType;
        var values = Enum.GetValues(enumTypeInUse);
        var searchList = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent($"{enumTypeInUse.Name}"), 0)
        };

        foreach (var value in values)
        {
            SearchTreeEntry treeEntry = new SearchTreeEntry(new GUIContent(value.ToString()))
            {
                level = 1,
                userData = value
            };
            searchList.Add(treeEntry);

        }

        return searchList;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        _onSelectEntryCallback?.Invoke((Enum)SearchTreeEntry.userData);
        return true;
    }
}
