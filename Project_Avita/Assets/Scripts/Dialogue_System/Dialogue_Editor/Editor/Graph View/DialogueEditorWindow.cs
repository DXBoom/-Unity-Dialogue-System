using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueEditorWindow : EditorWindow
{
    private DIalogueContainerSO currentDialogueContainer;
    private DialogueGraphView graphView;

    private LanguageType languageType = LanguageType.Russian;
    private ToolbarMenu toolbarMenu;
    private Label nameOfDialogueContainer;

    public LanguageType LanguageType { get => languageType; set => languageType = value; }

    [OnOpenAsset(1)]
    public static bool ShowWindow(int _instanceId, int line)
    {
        UnityEngine.Object item = EditorUtility.InstanceIDToObject(_instanceId);

        if (item is DIalogueContainerSO)
        {
            DialogueEditorWindow window = (DialogueEditorWindow)GetWindow(typeof(DialogueEditorWindow));
            window.titleContent = new GUIContent("Dialogue Editor");
            window.currentDialogueContainer = item as DIalogueContainerSO;
            window.minSize = new Vector2(500, 250);
            window.Load();
        }

        return false;
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolBar();
        Load();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void ConstructGraphView()
    {
        graphView = new DialogueGraphView(this);
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolBar()
    {
        StyleSheet styleSheet = Resources.Load<StyleSheet>("GraphViewStyleSheet");
        rootVisualElement.styleSheets.Add(styleSheet);

        Toolbar toolbar = new Toolbar();

        // Save button
        Button saveBtn = new Button()
        {
            text = "Save"
        };
        saveBtn.clicked += () =>
        {
            Save();
        };
        toolbar.Add(saveBtn);

        // Load button
        Button loadBtn = new Button()
        {
            text = "Load"
        };
        loadBtn.clicked += () =>
        {
            Load();
        };
        toolbar.Add(loadBtn);

        // Dropdown menu for language
        toolbarMenu = new ToolbarMenu();
        foreach (LanguageType language  in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
        {
            toolbarMenu.menu.AppendAction(language.ToString(), new Action<DropdownMenuAction>(x => Language(language, toolbarMenu)));
        }
        toolbar.Add(toolbarMenu);

        // Name of current DialogueContainer you have open.
        nameOfDialogueContainer = new Label("");
        toolbar.Add(nameOfDialogueContainer);
        nameOfDialogueContainer.AddToClassList("nameOfDialogueContainer");

        rootVisualElement.Add(toolbar);
    }

    private void Load()
    {
        // TODO: Load it
        Debug.Log("Load");
        if (currentDialogueContainer != null)
        {
            Language(LanguageType.Russian, toolbarMenu);
            nameOfDialogueContainer.text = "Name: " + currentDialogueContainer;
        }
    }

    private void Save()
    {
        // TODO: Save it
        Debug.Log("Save");
    }

    private void Language(LanguageType language, ToolbarMenu toolbarMenu)
    {
        // TODO: Language
        toolbarMenu.text = "Language: " + language.ToString();
        
    }
}
