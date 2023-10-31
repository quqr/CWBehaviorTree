using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class MyGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<MyGraphView, UxmlTraits>
    { }

    public MyGraphView()
    {
        InitWindown();
        CreateSearchWindow();
        AddRootNode();
    }

    private void AddRootNode()
    {
        CWNode rootNode = new RootNode();
        CreateNode(rootNode, Vector2.one);
    }

    private void InitWindown()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("Uss/GridBG"));

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        GridBackground grid = new GridBackground();
        Insert(0, grid);
    }

    public void SaveAsset(string name)
    {
        NodeGraphSaveTools saveTools = NodeGraphSaveTools.GetInstance(this);
        saveTools.SaveAsset(name);
    }

    public void LoadAsset(string name)
    {
        NodeGraphSaveTools saveTools = NodeGraphSaveTools.GetInstance(this);
        saveTools.LoadAsset(name);
    }

    //获得可连接的节点
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        ports.ForEach(port =>
        {
            if (startPort.node != port.node && startPort.direction != port.direction)
            {
                compatiblePorts.Add(port);
            }
        });
        return compatiblePorts;
    }

    //连接线
    public CWEdge CreateEdge(Port inPort, Port outPort)
    {
        CWEdge edge = new CWEdge { input = inPort, output = outPort };
        edge.input.Connect(edge);
        edge.output.Connect(edge);
        AddElement(edge);

        return edge;
    }

    public CWNode CreateNode(CWNode node, Vector2 position)
    {
        node.SetPosition(new Rect(position, Vector2.one));
        AddElement(node);
        return node;
    }

    public void CreateSearchWindow()
    {
        MySearchWindow searchWindow = ScriptableObject.CreateInstance<MySearchWindow>();
        searchWindow.OnSelectEntryHandler = OnMenuSelectEntry;

        nodeCreationRequest = context =>
        {
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        };
    }

    private bool OnMenuSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        var graphMousePosition = context.screenMousePosition;
        var type = searchTreeEntry.userData as Type;
        var node = Activator.CreateInstance(type) as CWNode;

        CreateNode(node, graphMousePosition);
        return true;
    }

    public void CleanGraphView()
    {
        foreach (Node node in nodes)
        {
            RemoveElement(node);
        }
        foreach (CWEdge edge in edges)
        {
            RemoveElement(edge);
        }
        AddRootNode();
    }
}