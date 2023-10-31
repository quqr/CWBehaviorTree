using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Class)]
public class VariablesAttribute : Attribute
{
    //�ڵ����·��
    public string VariablesPath { get; set; }

    public VariablesAttribute(string path)
    {
        VariablesPath = path;
    }
}
