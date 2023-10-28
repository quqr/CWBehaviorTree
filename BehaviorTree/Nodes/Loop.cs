using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CWNode("CWNode/Loop", "Loop")]
public class Loop : CWNode
{
    public Loop():base() 
    {
        SetStyle("Loop");
        SetPort();
    }
    //循环子节点
    public override NodeStates Evaluate()
    {
        foreach (CWNode node in ChildrenNode)
        {
            node.Evaluate();
        }
        return state;
    }

    public override void OnSelected()
    {
        base.OnSelected();
    }
}
