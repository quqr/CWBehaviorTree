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
        foreach (CWNode child in ChildrenNode)
        {
            child.Evaluate();
        }
        return NodeStates.RUNNING;
    }

    public override void OnSelected()
    {
        base.OnSelected();
    }
}