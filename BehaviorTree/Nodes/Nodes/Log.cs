using UnityEngine;

[CWNode("CWNode/Log", "Log")]
public class Log : CWNode
{
    public Log() : base()
    {
        SetStyle("Log");
        SetInputPort();
    }

    public override NodeStates Evaluate()
    {
        var message = BlackBoardDatas.Variables[0] as VariableString;
        Debug.Log(message.Value);
        return NodeStates.SUCCESS;
    }

    public override void OnSelected()
    {
        base.OnSelected();
    }
}