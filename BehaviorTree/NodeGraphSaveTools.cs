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
    BehaviourContainer loadedAsset;
    public static NodeGraphSaveTools GetInstance(MyGraphView graphView)
    {
        return new NodeGraphSaveTools { _graphView = graphView };
    }

    public void SaveAsset(string fileName)
    {
        BehaviourContainer obj = ScriptableObject.CreateInstance<BehaviourContainer>();
        loadedAsset = AssetDatabase.LoadAssetAtPath($"Assets/Resources/BehaviourTrees/{fileName}.asset", typeof(BehaviourContainer)) as BehaviourContainer;
        if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset))
        {
            AssetDatabase.CreateAsset(obj, $"Assets/Resources/BehaviourTrees/{fileName}.asset");
            return;
        }
        if (!SaveNodes(obj, fileName))
            return;
        //SaveNodeProperties(obj);

        //loadedAsset = AssetDatabase.LoadAssetAtPath($"Assets/Resources/BehaviourTrees/{fileName}.asset", typeof(BehaviourContainer)) as BehaviourContainer;
        loadedAsset.NodeLinkDatas = obj.NodeLinkDatas;
        loadedAsset.NodeDatas = obj.NodeDatas;
        loadedAsset.NodePropertyDatas = obj.NodePropertyDatas;
        EditorUtility.SetDirty(loadedAsset);

        AssetDatabase.SaveAssets();
    }

    private void SaveNodeProperties(BehaviourContainer obj)
    {
        throw new NotImplementedException();
    }

    private bool SaveNodes(BehaviourContainer obj, string fileName)
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
        //CleanAsset();
        loadedAsset.NodeInspectors.Clear();
        loadedAsset.NodeBlackBoards.Clear();
        loadedAsset.NodeVariables.Clear();
        foreach (CWNode node in nodes)
        {
            obj.NodeDatas.Add(new CWNodeData()
            {
                NodeGUID = node.GUID,
                Position = node.GetPosition().position,
                NodeTypeName = node.GetType().Name,

                Expansion = node.InspectorDatas.Expansion,
                NodeType = node.InspectorDatas.NodeType,
                NodeName = node.InspectorDatas.NodeName,

                VariableNames = node.BlackBoardDatas.VariableNames,
                Variables = node.BlackBoardDatas.Variables,
                VariableTypes = node.BlackBoardDatas.VariableTypes,

                Inspector = node.InspectorDatas,
                BlackBoard = node.BlackBoardDatas,
            });
            AddObjectToAsset(node);

        }
        return true;
    }

    //private void CleanAsset()
    //{
    //    foreach (var item in loadedAsset.NodeInspectors)
    //    {
    //        AssetDatabase.RemoveObjectFromAsset(item);
    //    }
    //    foreach (var item in loadedAsset.NodeBlackBoards)
    //    {
    //        AssetDatabase.RemoveObjectFromAsset(item);
    //    }
    //    loadedAsset.NodeInspectors.Clear();
    //    loadedAsset.NodeBlackBoards.Clear();
    //}

    void AddObjectToAsset(CWNode node)
    {
        foreach (var Variable in node.BlackBoardDatas.Variables)
        {
            AssetDatabase.RemoveObjectFromAsset(Variable);
            AssetDatabase.AddObjectToAsset(Variable, loadedAsset);
            loadedAsset.NodeVariables.Add(Variable);
        }
        AssetDatabase.RemoveObjectFromAsset(node.InspectorDatas);
        AssetDatabase.RemoveObjectFromAsset(node.BlackBoardDatas);
        AssetDatabase.AddObjectToAsset(node.InspectorDatas, loadedAsset);
        AssetDatabase.AddObjectToAsset(node.BlackBoardDatas, loadedAsset);

        loadedAsset.NodeInspectors.Add(node.InspectorDatas);
        loadedAsset.NodeBlackBoards.Add(node.BlackBoardDatas);
    }
    public void LoadAsset(string name)
    {
        BehaviourContainer _container = Resources.Load<BehaviourContainer>("BehaviourTrees/" + name);
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
        List<VariablesReference> variablesReferences = new List<VariablesReference>();
        CWNode _node;
        foreach (var item in datas)
        {
            _node = Activator.CreateInstance(Type.GetType(item.NodeTypeName)) as CWNode;
            _node.GUID = item.NodeGUID;
            _node.InspectorDatas = item.Inspector;
            _node.BlackBoardDatas = item.BlackBoard;
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