using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueNode : BaseNode
{
    private List<LanguageGeneric<string>> texts = new List<LanguageGeneric<string>>();
    private List<LanguageGeneric<AudioClip>> audioClips = new List<LanguageGeneric<AudioClip>>();
    private string name = "";
    private Sprite backgroundImage;
    private DialogueBackgroundImageType BackgroundImageType;

    public List<LanguageGeneric<string>> Texts { get => texts; set => texts = value; }
    public List<LanguageGeneric<AudioClip>> AudioClips { get => audioClips; set => audioClips = value; }
    public Sprite BackgroundImage { get => backgroundImage; set => backgroundImage = value; }
    public string Name { get => name; set => name = value; }
    public DialogueBackgroundImageType BackgroundImageType1 { get => BackgroundImageType; set => BackgroundImageType = value; }

    private TextField texts_Field;
    private ObjectField audioClips_Field;
    private ObjectField sprite_Field;
    private TextField name_Field;
    private EnumField backImageType_Field;

    public DialogueNode()
    {

    }

    public DialogueNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView)
    {
        editorWindow = _editorWindow;
        graphView = _graphView;

        title = "Dialogue";
        SetPosition(new Rect(_position, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();

        AddInputPort("Input", Port.Capacity.Multi);

        foreach (LanguageType language  in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
        {
            texts.Add(new LanguageGeneric<string> 
            {
                LanguageType = language,
                LanguageGenericType = ""
            });

            audioClips.Add(new LanguageGeneric<AudioClip>
            {
                LanguageType = language,
                LanguageGenericType = null
            });
        }

        texts_Field = new TextField("");
        texts_Field.RegisterValueChangedCallback(value =>
        {
            texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue;
        });
        texts_Field.SetValueWithoutNotify(texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType);
        texts_Field.multiline = true;

        texts_Field.AddToClassList("TextBox");
        mainContainer.Add(texts_Field);
    }
}
