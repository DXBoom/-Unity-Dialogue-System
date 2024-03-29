using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTalk : DialogueGetData
{
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private AudioSource audioSource;

    private DialogueNodeData currentDialogueNodeData;
    private DialogueNodeData lastDialogueNodeData;

    private void Awake()
    {
        dialogueController = FindObjectOfType<DialogueController>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartDialogue()
    {
        CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
        dialogueController.ShowDialogue(true);
    }

    private void CheckNodeType(BaseNodeData _baseNodeData)
    {
        switch (_baseNodeData)
        {
            case StartNodeData nodeData:
                RunNode(nodeData);
                break;
            case DialogueNodeData nodeData:
                RunNode(nodeData);
                break;
            case EventNodeData nodeData:
                RunNode(nodeData);
                break;
            case EndNodeData nodeData:
                RunNode(nodeData);
                break;
            default:
                break;
        }
    }

    private void RunNode(StartNodeData _nodeData)
    {
        CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
    }

    private void RunNode(DialogueNodeData _nodeData)
    {
        lastDialogueNodeData = currentDialogueNodeData;
        currentDialogueNodeData = _nodeData;

        dialogueController.SetText(_nodeData.Name, _nodeData.TextType.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
        dialogueController.SetImage(_nodeData.Sprite);

        MakeButtons(_nodeData.DialogueNodePorts);

        audioSource.clip = _nodeData.AudioClips.Find(clip => clip.LanguageType == LanguageController.Instance.Language).LanguageGenericType;
        audioSource.Play();
    }

    private void RunNode(EventNodeData _nodeData)
    {
        if(_nodeData.DialogueEventSO != null)
        {
            _nodeData.DialogueEventSO.RunEvent();
        }
        CheckNodeType(GetNextNode(_nodeData));
    }

    private void RunNode(EndNodeData _nodeData)
    {
        switch (_nodeData.EndNodeType)
        {
            case EndNodeType.End:
                dialogueController.ShowDialogue(false);
                break;
            case EndNodeType.Repeat:
                CheckNodeType(GetNodeByGuid(currentDialogueNodeData.NodeGuid));
                break;
            case EndNodeType.Goback:
                CheckNodeType(GetNodeByGuid(lastDialogueNodeData.NodeGuid));
                break;
            case EndNodeType.ReturnToStart:
                CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
                break;
            default:
                break;
        }
    }

    private void MakeButtons(List<DialogueNodePort> _nodePorts)
    {
        List<string> texts = new List<string>();
        List<UnityAction> unityActions = new List<UnityAction>();

        foreach (DialogueNodePort nodePort in _nodePorts)
        {
            texts.Add(nodePort.TextLanguages.Find(text => text.LanguageType == LanguageController.Instance.Language).LanguageGenericType);
            UnityAction tempAction = null;
            tempAction += () =>
            {
                CheckNodeType(GetNodeByGuid(nodePort.InputGuid));
                audioSource.Stop();
            };
            unityActions.Add(tempAction);
        }

        dialogueController.SetButtons(texts, unityActions);
    }
}
