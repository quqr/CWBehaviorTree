using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CWNode : Node
{
    public enum NodeStates
    {
        SUCCESS,
        FAILURE,
        RUNNING
    }

    public string GUID;
    public string NodeName = "Node";
    public List<CWNode> ChildrenNode = new List<CWNode>();
    public List<string> ChildrenNodeGUID = new List<string>();

    private Port inputPort;
    private Port outputPort;
    private VisualElement nodeType;
    private Label nodeName;

    protected NodeStates state;
    protected CWNode nodeParent;
    public string NodeTypeName;

    public CWNode()
    {
        GUID = Guid.NewGuid().ToString();
        nodeParent = null;
    }

    public virtual NodeStates Evaluate()
    {
        return NodeStates.FAILURE;
    }

    public virtual void SetPort()
    {
        inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(Node));
        inputPort.portName = null;
        inputContainer.Add(inputPort);

        outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(Node));
        outputPort.portName = null;
        outputContainer.Add(outputPort);

        RefreshExpandedState();
        RefreshPorts();
    }

    public void SetRootPort()
    {
        outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(Node));
        outputPort.portName = null;
        outputContainer.Add(outputPort);

        RefreshExpandedState();
        RefreshPorts();
    }

    public void SetStyle(string nodeTypeName)
    {
        styleSheets.Add(Resources.Load<StyleSheet>("Uss/" + nodeTypeName));

        this.NodeTypeName = nodeTypeName;

        nodeName = new Label();
        nodeName.name = "nodeName";
        nodeName.text = nodeTypeName;

        var top = mainContainer.Q("top");

        top.style.flexDirection = FlexDirection.Column;
        nodeType = new VisualElement();
        nodeType.name = nodeTypeName;
        nodeType.Add(nodeName);

        top.Insert(1, nodeType);

        RefreshExpandedState();
        RefreshPorts();
    }

    public CWNodeBaseInformation.Inspector InspectorDatas = ScriptableObject.CreateInstance<CWNodeBaseInformation.Inspector>();
    public CWNodeBaseInformation.BlackBoard BlackBoardDatas = ScriptableObject.CreateInstance<CWNodeBaseInformation.BlackBoard>();
   
    public override void OnSelected()
    {
        CWNodeDataFactor.Instance.NodeGUID = GUID;
        CWNodeDataFactor.Instance.AddInformationToInspector(InspectorDatas);
        CWNodeDataFactor.Instance.AddInformationToBlackBoard(BlackBoardDatas);
    }
}