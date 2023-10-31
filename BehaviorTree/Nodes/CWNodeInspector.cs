using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class CWNodeInspector : ScriptableObject
{
    public MonoScript Expansion;
    public string NodeType;
    public string NodeName;
}