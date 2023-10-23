using System;

[AttributeUsage(AttributeTargets.Class)]
public class CWNodeAttribute : Attribute
{
    //�ڵ����·��
    public string NodePath { get; set; }

    //�ڵ�����
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