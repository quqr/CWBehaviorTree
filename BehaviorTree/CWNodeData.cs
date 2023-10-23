using System;
using UnityEngine;

[Serializable]
public class CWNodeData
{
    public string NodeGUID;
    public Vector2 Position;
    public string NodeTypeName;
    public CWNodeBaseInformation.Inspector InspectorDatas;
    public CWNodeBaseInformation.BlackBoard BlackBoardDatas;
}