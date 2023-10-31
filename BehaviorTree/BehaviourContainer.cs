using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BehaviourContainer : ScriptableObject
{
    public List<CWNodeData> NodeDatas = new List<CWNodeData>();
    public List<CWNodeLinkData> NodeLinkDatas = new List<CWNodeLinkData>();
    public List<CWNodeProperty> NodePropertyDatas = new List<CWNodeProperty>();

    public List<CWNodeInspector> NodeInspectors = new List<CWNodeInspector>();
    public List<CWNodeBlackBoard> NodeBlackBoards = new List<CWNodeBlackBoard>();
    public List<VariablesReference> NodeVariables = new List<VariablesReference>();
}