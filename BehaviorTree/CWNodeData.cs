using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class CWNodeData
{
    public string NodeGUID;
    public Vector2 Position;
    public string NodeTypeName;

    public MonoScript Expansion;
    public string NodeType;
    public string NodeName;

    public List<string> VariableNames = new List<string>();
    public List<string> VariableTypes = new List<string>();
    public List<VariablesReference> Variables = new List<VariablesReference>();

    public CWNodeInspector Inspector;
    public CWNodeBlackBoard BlackBoard;

}