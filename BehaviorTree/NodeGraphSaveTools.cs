using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeGraphSaveTools
{
    private MyGraphView _graphView;
    private List<Edge> edges => _graphView.edges.ToList();
    private List<CWNode> nodes => _graphView.nodes.ToList().Cast<CWNode>().ToList();

    public static NodeGraphSaveTools GetInstance(MyGraphView graphView)
    {
        return new NodeGraphSaveTools { _graphView = graphView };
    }

    public void SaveAsset(string fileName)
    {
        BehaviourContainer obj = ScriptableObject.CreateInstance<BehaviourContainer>();
        if (!SaveNodes(obj))
            return;
        //SaveNodeProperties(obj);

        UnityEngine.Object loadedAsset = AssetDatabase.LoadAssetAtPath($"Assets/Resources/{fileName}.asset", typeof(BehaviourContainer));

        if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset))
        {
            AssetDatabase.CreateAsset(obj, $"Assets/Resources/{fileName}.asset");
        }
        else
        {
            BehaviourContainer container = loadedAsset as BehaviourContainer;

            container.NodeLinkDatas = obj.NodeLinkDatas;
            container.NodeDatas = obj.NodeDatas;
            container.NodePropertyDatas = obj.NodePropertyDatas;
            EditorUtility.SetDirty(container);
        }

        AssetDatabase.SaveAssets();
    }

    private void SaveNodeProperties(BehaviourContainer obj)
    {
        throw new NotImplementedException();
    }

    private bool SaveNodes(BehaviourContainer obj)
    {
        var connectedSockets = edges.Where(x => x.input.node != null).ToArray();

        foreach (var connectedSocket in connectedSockets)
        {
            var outputNode = (connectedSocket.output.node as CWNode);
            var inputNode = (connectedSocket.input.node as CWNode);
            obj.NodeLinkDatas.Add(new CWNodeLinkData
            {
                BasePortGUID = outputNode.GUID,
                TargetGUID = inputNode.GUID,
            });
        }

        foreach (CWNode node in nodes)
        {
            obj.NodeDatas.Add(new CWNodeData()
            {
                NodeGUID = node.GUID,
                Position = node.GetPosition().position,
                NodeTypeName = node.GetType().Name,
                InspectorDatas = node.InspectorDatas,
                BlackBoardDatas = node.BlackBoardDatas
            });
        }
        return true;
    }

    public void LoadAsset(string name)
    {
        BehaviourContainer _container = Resources.Load<BehaviourContainer>(name);
        if (_container == null)
            return;
        CleanGraphView();
        CreatNodes(_container);
        LinkNodes(_container);
        //SetProp();
    }

    public void CleanGraphView()
    {
        foreach (Node node in nodes)
        {
            _graphView.RemoveElement(node);
        }
        foreach (Edge edge in edges)
        {
            _graphView.RemoveElement(edge);
        }
    }

    private void SetProp()
    {
        throw new NotImplementedException();
    }

    private void LinkNodes(BehaviourContainer _container)
    {
        for (int i = 0; i < nodes.Count(); i++)
        {
            var connections = _container.NodeLinkDatas.Where(x => x.BasePortGUID == nodes[i].GUID).ToList();
            for (int j = 0; j < connections.Count; j++)
            {
                var targetNodeGUID = connections[j].TargetGUID;
                var targetNode = nodes.First(x => x.GUID == targetNodeGUID);

                _graphView.CreateEdge((Port)targetNode.inputContainer[0], nodes[i].outputContainer[0].Q<Port>());
            }
        }
    }

    private void CreatNodes(BehaviourContainer _container)
    {
        List<CWNodeData> datas = _container.NodeDatas;
        List<CWNodeLinkData> linkDatas = _container.NodeLinkDatas;
        CWNode _node;
        foreach (var item in datas)
        {
            _node = Activator.CreateInstance(Type.GetType(item.NodeTypeName)) as CWNode;
            _node.GUID = item.NodeGUID;
            _node.InspectorDatas = item.InspectorDatas;
            _node.BlackBoardDatas = item.BlackBoardDatas;
            foreach (var GUIDS in linkDatas)
            {
                if (_node.GUID == GUIDS.BasePortGUID)
                {
                    _node.ChildrenNodeGUID.Add(GUIDS.TargetGUID);
                }
            }
            _graphView.CreateNode(_node, item.Position);
        }

        foreach (var node in nodes)
        {
            foreach (var childrenNodeGUID in node.ChildrenNodeGUID)
            {
                node.ChildrenNode.AddRange(
                    nodes.Where(x => x.GUID == childrenNodeGUID).ToList()
                    );
            }
        }
    }
}