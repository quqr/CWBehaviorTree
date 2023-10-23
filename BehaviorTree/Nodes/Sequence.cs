using UnityEngine;

[CWNode("CWNode/Sequence", "Sequence")]
public class Sequence : CWNode
{
    public Sequence() : base()
    {
        SetStyle("Sequence");
        SetPort();
    }

    //һ�������ӽڵ㷵��ʧ�ܵ�״̬��������ֹͣ������
    public override NodeStates Evaluate()
    {
        Debug.Log("Selector");

        bool anyNodeRunning = false;

        foreach (CWNode node in ChildrenNode)
        {
            switch (node.Evaluate())
            {
                case NodeStates.SUCCESS:
                    state = NodeStates.SUCCESS;
                    continue;

                case NodeStates.FAILURE:
                    state = NodeStates.FAILURE;
                    return state;

                case NodeStates.RUNNING:
                    anyNodeRunning = true;
                    state = NodeStates.RUNNING;
                    continue;
                default:
                    state = NodeStates.SUCCESS;
                    continue;
            }
        }
        state = anyNodeRunning ? NodeStates.RUNNING : NodeStates.SUCCESS;
        return state;
    }

    public override void OnSelected()
    {
        base.OnSelected();
    }
}