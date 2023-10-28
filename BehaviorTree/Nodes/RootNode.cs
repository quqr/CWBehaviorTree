using System;
using UnityEngine;

[CWNode("CWNode/RootNode", "RootNode")]
public class RootNode : CWNode
{
    public RootNode() : base()
    {
        SetStyle("RootNode");
        SetRootPort();
    }

    public override NodeStates Evaluate()
    {

        CWNode loopNode=new CWNode();
        if (loopNode.NodeTypeName == "Loop")
        {
            loopNode.Evaluate();
            return NodeStates.RUNNING;
        }

        foreach (CWNode child in ChildrenNode)
        {
            if (child.NodeTypeName=="Loop")
            {
                loopNode = child;
            }
            child.Evaluate();
        }
        return NodeStates.RUNNING;
    }

    public override void OnSelected()
    {
        base.OnSelected();
    }
}