using System;
using UnityEngine;

[Serializable]
public class Variable<T> : VariablesReference
{
    protected string VariableName;
    protected Type type = typeof(T);
    public T Value = default(T);

    public void SetValue(T value)
    {
        Value = value;
    }

    public T GetValue()
    {
        return Value;
    }
}