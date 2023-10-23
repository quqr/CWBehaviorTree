using UnityEngine;

public abstract class MyBehaviourTree : MonoBehaviour
{
    [SerializeField]
    protected CWNode rootNode;

    private void Start()
    {
        rootNode = SetupRoot();
    }

    protected abstract CWNode SetupRoot();

    private void Update()
    {
        if (rootNode != null)
        {
            rootNode.Evaluate();
        }
    }
}