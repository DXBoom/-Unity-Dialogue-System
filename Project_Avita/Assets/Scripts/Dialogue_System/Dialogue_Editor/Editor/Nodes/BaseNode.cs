using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseNode : Node
{
    private string nodeGuid;
    protected DialogueGraphView graphView;
    protected DialogueEditorWindow editorWindow;
    protected Vector2 defaultNodeSize = new Vector2(200, 250);

    protected string NodeGuid { get => nodeGuid; set => nodeGuid = value; }

    public BaseNode()
    {
        StyleSheet styleSheet = Resources.Load<StyleSheet>("NodeStyleSheet");
        styleSheets.Add(styleSheet);
    }

    public void AddOutputPort(string name, Port.Capacity capacity = Port.Capacity.Single)
    {
        Port outputPort = GetPortInstance(Direction.Output, capacity);
        outputPort.name = name;
        outputContainer.Add(outputPort);
    }

    public void AddInputPort(string name, Port.Capacity capacity = Port.Capacity.Multi)
    {
        Port inputPort = GetPortInstance(Direction.Input, capacity);
        inputPort.name = name;
        inputContainer.Add(inputPort);
    }

    public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
    }
}