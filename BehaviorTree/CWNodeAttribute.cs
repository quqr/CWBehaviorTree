using System;

[AttributeUsage(AttributeTargets.Class)]
public class CWNodeAttribute : Attribute
{
    //节点分类路径
    public string NodePath { get; set; }

    //节点描述
    public string NodeDescription { get; set; } = "";

    public CWNodeAttribute(string nodepath)
    {
        NodePath = nodepath;
    }

    public CWNodeAttribute(string nodepath, string description)
    {
        NodePath = nodepath;
        NodeDescription = description;
    }
}