using UnityEngine;

[CWNode("CWNode/Selector", "Selector")]
public class Selector : CWNode
{
    public Selector() : base()
    {
        SetStyle("Selector");
        SetPort();
    }

    //遍历所有的子节点，一旦有子节点满足条件执行的行为，遍历马上终止掉。
    public override NodeStates Evaluate()
    {
        Debug.Log("Selector");
        foreach (CWNode node in ChildrenNode)
        {
            switch (node.Evaluate())
            {
                case NodeStates.SUCCESS:
                    state = NodeStates.SUCCESS;
                    return NodeStates.SUCCESS;

                case NodeStates.FAILURE:
                    state = NodeStates.FAILURE;
                    continue;

                case NodeStates.RUNNING:
                    state = NodeStates.RUNNING;
                    return NodeStates.RUNNING;

                default: continue;
            }
        }
        state = NodeStates.FAILURE;
        return state;
    }

    public override void OnSelected()
    {
        base.OnSelected();
    }
}