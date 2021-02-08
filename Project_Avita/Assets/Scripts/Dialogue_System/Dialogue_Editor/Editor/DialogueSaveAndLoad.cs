using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogueSaveAndLoad
{
    private DialogueGraphView graphView;
    private List<Edge> edges => graphView.edges.ToList();
    private List<BaseNode> nodes => graphView.nodes.ToList().Where(Node => Node is BaseNode).Cast<BaseNode>().ToList();

    public DialogueSaveAndLoad(DialogueGraphView _graphView)
    {
        graphView = _graphView;
    }

    public void Save(DialogueContainerSO _dialogueContainerSO)
    {
        SaveEdges(_dialogueContainerSO);
        SaveNodes(_dialogueContainerSO);

        EditorUtility.SetDirty(_dialogueContainerSO);
        AssetDatabase.SaveAssets();
    }

    public void Load(DialogueContainerSO _dialogueContainerSO)
    {

    }

    private void SaveEdges(DialogueContainerSO _dialogueContainerSO)
    {
        _dialogueContainerSO.NodeLinkDatas.Clear();

        Edge[] connectedEdges = edges.Where(edge => edge.input.node != null).ToArray();
        for (int i = 0; i < connectedEdges.Count(); i++)
        {
            BaseNode outputNode = (BaseNode)connectedEdges[i].output.node;
            BaseNode inputNode = connectedEdges[i].input.node as BaseNode;

            _dialogueContainerSO.NodeLinkDatas.Add(new NodeLinkData 
            {
                BaseNodeGuid = outputNode.NodeGuid,
                TargetNodeGuid = inputNode.NodeGuid
            });
        }
    }

    private void SaveNodes(DialogueContainerSO _dialogueContainerSO)
    {
        _dialogueContainerSO.DialogueNodeDatas.Clear();
        _dialogueContainerSO.EventNodeDatas.Clear();
        _dialogueContainerSO.EndNodeDatas.Clear();
        _dialogueContainerSO.StartNodeDatas.Clear();

        nodes.ForEach(node =>
        {
            switch (node)
            {
                case DialogueNode dialogueNode:
                    _dialogueContainerSO.DialogueNodeDatas.Add(SaveNodeData(dialogueNode));
                    break;
                case StartNode startNode:
                    _dialogueContainerSO.StartNodeDatas.Add(SaveNodeData(startNode));
                    break;
                case EndNode endNode:
                    _dialogueContainerSO.EndNodeDatas.Add(SaveNodeData(endNode));
                    break;
                case EventNode eventNode:
                    _dialogueContainerSO.EventNodeDatas.Add(SaveNodeData(eventNode));
                    break;
                default:
                    break;
            }
        });
    }

    private DialogueNodeData SaveNodeData(DialogueNode _node)
    {
        DialogueNodeData dialogueNodeData = new DialogueNodeData
        {
            NodeGuid = _node.NodeGuid,
            Position = _node.GetPosition().position,
            TextType = _node.Texts,
            Name = _node.name,
            AudioClips = _node.AudioClips,
            DialogueBackgroundImageType = _node.BackgroundImageType,
            Sprite = _node.BackgroundImage,
            DialogueNodePorts = _node.DialogueNodePorts
        };

        foreach (DialogueNodePort nodePort in dialogueNodeData.DialogueNodePorts)
        {
            foreach (Edge edge in edges)
            {
                if(edge.output == nodePort.MyPort)
                {
                    nodePort.OutputGuid = (edge.output.node as BaseNode).NodeGuid;
                    nodePort.InputGuid = (edge.input.node as BaseNode).NodeGuid;
                }
            }
        }

        return dialogueNodeData;
    }

    private StartNodeData SaveNodeData(StartNode _node)
    {
        StartNodeData nodeData = new StartNodeData()
        {
            NodeGuid = _node.NodeGuid,
            Position = _node.GetPosition().position,
        };

        return nodeData;
    }

    private EndNodeData SaveNodeData(EndNode _node)
    {
        EndNodeData nodeData = new EndNodeData()
        {
            NodeGuid = _node.NodeGuid,
            Position = _node.GetPosition().position,
            EndNodeType = _node.EndNodeType
        };

        return nodeData;
    }

    private EventNodeData SaveNodeData(EventNode _node)
    {
        EventNodeData nodeData = new EventNodeData()
        {
            NodeGuid = _node.NodeGuid,
            Position = _node.GetPosition().position,
            DialogueEventSO = _node.DialogueEvent
        };

        return nodeData;
    }
}