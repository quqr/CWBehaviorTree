using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourTree : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    VisualElement main;
    public VisualElement inspector;
    [MenuItem("MyTools/BehaviorTree")]
    public static void ShowExample()
    {
        BehaviourTree wnd = GetWindow<BehaviourTree>();
        wnd.titleContent = new GUIContent("BehaviorTree");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        main = root.Q("Main");
        main.style.marginBottom = -800f;

        inspector = root.Q("Inspector");
    }
    
}
