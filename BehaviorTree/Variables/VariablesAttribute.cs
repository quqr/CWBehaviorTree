using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Class)]
public class VariablesAttribute : Attribute
{
    //节点分类路径
    public string VariablesPath { get; set; }

    public VariablesAttribute(string path)
    {
        VariablesPath = path;
    }
}
