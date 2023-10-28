using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity;
using UnityEngine.Windows;

[Serializable]
public class CWNodeBaseInformation
{
    [Serializable]
    public class Inspector : ScriptableObject
    {
        public MonoScript Expansion;
        public string NodeType;
        public string NodeName;
    }

    [Serializable]
    public class BlackBoard : ScriptableObject
    {
        public List<string> VariableNames = new List<string>();
        public List<VariablesReference> Variables = new List<VariablesReference>();
    }
}