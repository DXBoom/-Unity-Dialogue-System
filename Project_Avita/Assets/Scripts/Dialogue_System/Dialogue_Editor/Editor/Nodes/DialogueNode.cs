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
    private DialogueBackgroundImageType backgroundImageType;

    public List<LanguageGeneric<string>> Texts { get => texts; set => texts = value; }
    public List<LanguageGeneric<AudioClip>> AudioClips { get => audioClips; set => audioClips = value; }
    public Sprite BackgroundImage { get => backgroundImage; set => backgroundImage = value; }
    public string Name { get => name; set => name = value; }
    public DialogueBackgroundImageType BackgroundImageType { get => BackgroundImageType; set => BackgroundImageType = value; }

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

        // Image 
        sprite_Field = new ObjectField
        {
            objectType = typeof(Sprite),
            allowSceneObjects = false,
            value = backgroundImage
        };
        sprite_Field.RegisterValueChangedCallback(value => 
        {
            backgroundImage = value.newValue as Sprite;
        });
        mainContainer.Add(sprite_Field);

        // Background Image Enum
        backImageType_Field = new EnumField()
        {
            value = backgroundImageType
        };
        backImageType_Field.Init(backgroundImageType);
        backImageType_Field.RegisterValueChangedCallback(value =>
        {
            backgroundImageType = (DialogueBackgroundImageType)value.newValue;
        });
        mainContainer.Add(sprite_Field);

        // Audio Clips
        audioClips_Field = new ObjectField()
        {
            objectType = typeof(AudioClip),
            allowSceneObjects = false,
            value = audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType,
        };
        audioClips_Field.RegisterValueChangedCallback(value =>
        {
            audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue as AudioClip;
        });
        audioClips_Field.SetValueWithoutNotify(audioClips.Find(audioClip => audioClip.LanguageType == editorWindow.LanguageType).LanguageGenericType);
        mainContainer.Add(audioClips_Field);

        // Text Name
        Label label_name = new Label("Name");
        label_name.AddToClassList("label_name");
        label_name.AddToClassList("Label");
        mainContainer.Add(label_name);

        name_Field = new TextField("Name");
        texts_Field.RegisterValueChangedCallback(value =>
        {
            name = value.newValue;
        });
        texts_Field.SetValueWithoutNotify(name);
        texts_Field.AddToClassList("TextName");
        mainContainer.Add(texts_Field);

        // Text Box
        Label label_texts = new Label("Text Box");
        label_texts.AddToClassList("label_texts");
        label_texts.AddToClassList("Label");
        mainContainer.Add(label_texts);

        texts_Field = new TextField("");
        texts_Field.RegisterValueChangedCallback(value =>
        {
            texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType = value.newValue;
        });
        texts_Field.SetValueWithoutNotify(texts.Find(text => text.LanguageType == editorWindow.LanguageType).LanguageGenericType);
        texts_Field.multiline = true;

        texts_Field.AddToClassList("TextBox");
        mainContainer.Add(texts_Field);

        Button button = new Button()
        {
            text = "Add Choice"
        };
        button.clicked += () =>
        {
            // TODO: Add a new Choice for output Port.
        };
    }
}
