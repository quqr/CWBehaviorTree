using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CWNodeBlackBoard : ScriptableObject
{
    public List<string> VariableNames = new List<string>();
    public List<VariablesReference> Variables = new List<VariablesReference>();
    public List<string> VariableTypes = new List<string>();
}