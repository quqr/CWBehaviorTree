using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class MyBehaviourTree : MonoBehaviour
{
    [SerializeField]
    protected CWNode rootNode;
    List<CWNode> nodes=new List<CWNode>();
    private void Start()
    {
        rootNode = SetupRoot();
    }

    protected abstract CWNode SetupRoot();

    private void Update()
    {
        if (rootNode != null)
        {
            rootNode.Evaluate();
        }
    }

    protected CWNode GetRootNode(BehaviourContainer asset)
    {
        return GetAllNodes(asset)[0];
    }
    protected List<CWNode> GetAllNodes(BehaviourContainer asset)
    {
        List<CWNodeData> NodeDatas = asset.NodeDatas;
        List<CWNodeLinkData> NodeLinkDatas = asset.NodeLinkDatas;
        List<CWNodeProperty> NodePropertyDatas = asset.NodePropertyDatas;

        foreach (CWNodeData item in NodeDatas)
        {
            CWNode _node = Activator.CreateInstance(Type.GetType(item.NodeTypeName)) as CWNode;
            _node.GUID = item.NodeGUID;

            _node.InspectorDatas = item.Inspector as CWNodeInspector;
            _node.BlackBoardDatas = item.BlackBoard as CWNodeBlackBoard;
            foreach (var GUIDS in NodeLinkDatas)
            {
                if (_node.GUID == GUIDS.BasePortGUID)
                {
                    _node.ChildrenNodeGUID.Add(GUIDS.TargetGUID);
                }
            }
            nodes.Add(_node);
        }
        foreach (CWNode node in nodes)
        {
            foreach (var childrenNodeGUID in node.ChildrenNodeGUID)
            {
                node.ChildrenNode.AddRange(
                    nodes.Where(x => x.GUID == childrenNodeGUID).ToList()
                    );
            }
        }

        return nodes;
    }
}