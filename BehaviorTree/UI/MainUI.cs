using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;
using Unity.VisualScripting;
using System.Collections.Generic;
using System;
using System.Linq;

public class MainUI : TwoPaneSplitView
{
    public new class UxmlFactory : UxmlFactory<MainUI, UxmlTraits>
    { }

    private VisualElement leftPane;
    private VisualElement rightPane;
    private VisualElement inspector;
    private VisualElement inspectorContent;
    private VisualElement blackBoard;
    private VisualElement blackBoardContent;
    private TextField assetName;
    private TwoPaneSplitView paneSplitView;
    private MyGraphView graphView;
    private Button saveAsset;
    private Button loadAsset;
    private Button newAsset;
    private DropdownField types;
    private Button deleteVariable;
    private Button addVariable;
    private TextField variableName;


    private VisualTreeAsset blackBoardTree = Resources.Load<VisualTreeAsset>("Uxml/Blackboard");
    private VisualTreeAsset inspectorTree = Resources.Load<VisualTreeAsset>("Uxml/Inspector");

    private List<string> dropTypes = new List<string>() {
        "Boolean","Float","Vector2","String"
    };
    private Dictionary<string, Type> variableDicionary = new Dictionary<string, Type>()
    {
        {"Boolean",typeof(VariableBool) },
        {"Float",typeof(VariableFloat) },
        {"Vector2",typeof(VariableVector2) },
        {"String",typeof(VariableString) },
    };
    public MainUI()
    {
        InitWindow();
        CWNodeDataFactor.Instance.Inspector = inspector;
        CWNodeDataFactor.Instance.BlackBoardContent = blackBoardContent;
        CWNodeDataFactor.Instance.blackBoardListView = blackBoardContent.Q<ListView>("variables");

    }

    private void InitWindow()
    {
        Initialize();

        SetProp();

        BindEvents();

        AddElements();
    }

    private void Initialize()
    {
        leftPane = new VisualElement();
        rightPane = new VisualElement();
        inspector = new VisualElement();
        blackBoard = new VisualElement();
        paneSplitView = new TwoPaneSplitView();
        graphView = new MyGraphView();
        assetName = new TextField();
        loadAsset = new Button();
        saveAsset = new Button();
        newAsset = new Button();
        blackBoardContent = blackBoardTree.CloneTree();
        inspectorContent = inspectorTree.CloneTree();

        types = blackBoardContent.Q<DropdownField>("variableType");
        addVariable = blackBoardContent.Q<Button>("add");
        variableName = blackBoardContent.Q<TextField>("variableName");
        deleteVariable=blackBoardContent.Q<Button>("delete");
    }

    private void SetProp()
    {
        saveAsset.text = "Save";
        loadAsset.text = "Load";
        newAsset.text = "New";

        leftPane.name = "leftPane";

        rightPane.name = "rightPane";
        assetName.name = "assetName";

        inspector.name = "Inspector";

        blackBoard.name = "BlackBoard";

        paneSplitView.name = "pane";
        paneSplitView.orientation = TwoPaneSplitViewOrientation.Vertical;

        assetName.style.flexDirection = FlexDirection.Row;

        graphView.name = "grid";
        blackBoardContent.name = "blackBoardContent";

        types.choices = dropTypes;
    }

    private void BindEvents()
    {
        newAsset.clicked += NewClicked;
        saveAsset.clicked += SaveClicked;
        loadAsset.clicked += LoadClicked;
        addVariable.clicked += AddVariableClicked;
        deleteVariable.clicked += DeleteVariableClicked;
    }

    private void DeleteVariableClicked()
    {
        foreach (CWNode node in graphView.nodes)
        {
            if (node.GUID == CWNodeDataFactor.Instance.NodeGUID)
            {
                Debug.Log(node.BlackBoardDatas.VariableNames.Count);
                Debug.Log(node.BlackBoardDatas.Variables.Count);
                node.BlackBoardDatas.VariableNames.RemoveAt(node.BlackBoardDatas.VariableNames.Count-1);
                node.BlackBoardDatas.Variables.RemoveAt(node.BlackBoardDatas.Variables.Count - 1);
                CWNodeDataFactor.Instance.AddInformationToBlackBoard(node.BlackBoardDatas);
                return;
            }
        }
    }

    private void AddElements()
    {
        Add(leftPane);
        leftPane.Add(paneSplitView);

        paneSplitView.Add(inspector);
        paneSplitView.Add(blackBoard);

        assetName.Add(newAsset);
        assetName.Add(saveAsset);
        assetName.Add(loadAsset);

        inspector.Add(assetName);
        Add(rightPane);
        rightPane.Add(graphView);

        inspector.Add(inspectorContent);
        blackBoard.Add(blackBoardContent);
    }
    private void AddVariableClicked()
    {
        string index = types.value;
        string name = variableName.value;

        if (name==string.Empty)
        {
            return;
        }

        Type variableType=null;
        try
        {
            variableType = variableDicionary[index];
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return;
        }

        VariablesReference obj = ScriptableObject.CreateInstance(variableType.Name) as VariablesReference;
        
        foreach (CWNode node in graphView.nodes)
        {
            if (node.GUID == CWNodeDataFactor.Instance.NodeGUID)
            {
                node.BlackBoardDatas.VariableNames.Add(name);
                node.BlackBoardDatas.Variables.Add(obj);
                node.BlackBoardDatas.VariableTypes.Add(variableType.Name);
                CWNodeDataFactor.Instance.AddInformationToBlackBoard(node.BlackBoardDatas);
                return;
            }
        }
    }
    private void NewClicked()
    {
        graphView.CleanGraphView();
        CWNodeDataFactor.Instance.AddInformationToBlackBoard(null);


    }

    private void LoadClicked()
    {
        graphView.LoadAsset(assetName.value);
        CWNodeDataFactor.Instance.AddInformationToBlackBoard(null);


    }

    private void SaveClicked()
    {
        graphView.SaveAsset(assetName.value);

    }
}