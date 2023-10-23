using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BehaviourContainer : ScriptableObject
{
    public List<CWNodeData> NodeDatas = new List<CWNodeData>();
    public List<CWNodeLinkData> NodeLinkDatas = new List<CWNodeLinkData>();
    public List<CWNodeProperty> NodePropertyDatas = new List<CWNodeProperty>();
}