using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class CWNodeDataFactor
{
    private static CWNodeDataFactor instance;
    public static CWNodeDataFactor Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CWNodeDataFactor();
            }
            return instance;
        }
    }

    public VisualElement Inspector;
    public VisualElement BlackBoardContent;
    public ListView blackBoardListView;

    public string NodeGUID;

    public void AddInformationToBlackBoard(CWNodeBaseInformation.BlackBoard BlackBoardDatas)
    {
        blackBoardListView = BlackBoardContent.Q<ListView>("variables");
        blackBoardListView.makeItem = () =>
        {
            VisualElement templateBlackBoardContainer = new VisualElement();
            templateBlackBoardContainer.name = "visualField";
            templateBlackBoardContainer.Add(new Label() { name = "variableName" });
            templateBlackBoardContainer.Add(new PropertyField() { name = "field" });
            return templateBlackBoardContainer;
        };
        blackBoardListView.bindItem = (item, index) =>
        {
            SerializedObject serializedObject = new SerializedObject(BlackBoardDatas.Variables[index]);

            SerializedProperty property = serializedObject.FindProperty("Value");
            item.Q<PropertyField>("field").label = BlackBoardDatas.VariableNames[index];
            item.Q<PropertyField>("field").BindProperty(property);
            item.Q<PropertyField>("field").Bind(serializedObject);
        };
        blackBoardListView.itemsSource = BlackBoardDatas.Variables;
    }
    VisualElement templateInspectorContainer = new VisualElement();
    public void AddInformationToInspector(CWNodeBaseInformation.Inspector InspectorDatas)
    {
        templateInspectorContainer.Clear();
        SerializedObject serializedNode = new SerializedObject(InspectorDatas);
        SerializedProperty nodeProperty = serializedNode.GetIterator();
        nodeProperty.Next(true);

        while (nodeProperty.NextVisible(false))
        {
            PropertyField field = new PropertyField(nodeProperty);
            field.Bind(serializedNode);
            templateInspectorContainer.Add(field);
            Inspector.Add(templateInspectorContainer);
        }
    }
}