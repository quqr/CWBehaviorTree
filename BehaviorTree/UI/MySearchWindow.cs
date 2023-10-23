using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class MySearchWindow : ScriptableObject, ISearchWindowProvider
{
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> searchTreeEntry = new List<SearchTreeEntry>();
        searchTreeEntry.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));

        List<Type> types = new List<Type>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            List<Type> result = assembly.GetTypes().Where(type =>
            {
                return type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(CWNode));
            }).ToList();
            types.AddRange(result);
        }
        foreach (Type type in types)
        {
            //获取节点属性的NodePath
            string nodePath = type.GetCustomAttribute<CWNodeAttribute>()?.NodePath;
            if (nodePath == null) continue;
            //将路径中每一项分割
            string[] menus = nodePath.Split('/');
            searchTreeEntry.Add(new SearchTreeEntry(new GUIContent(menus[1]))
            {
                level = 1,
                userData = type
            });
        }

        return searchTreeEntry;
    }

    public delegate bool SerchMenuWindowOnSelectEntryDelegate(SearchTreeEntry searchTreeEntry,
          SearchWindowContext context);

    public SerchMenuWindowOnSelectEntryDelegate OnSelectEntryHandler;//delegate回调方法

    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        if (OnSelectEntryHandler == null)
        {
            return false;
        }
        return OnSelectEntryHandler(searchTreeEntry, context);
    }

}
