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
    private DialogueContainerSO currentDialogueContainer;
    private DialogueGraphView graphView;
    private DialogueSaveAndLoad saveAndLoad;

    private LanguageType languageType = LanguageType.Russian;
    private ToolbarMenu toolbarMenu;
    private Label nameOfDialogueContainer;

    public LanguageType LanguageType { get => languageType; set => languageType = value; }

    [OnOpenAsset(1)]
    public static bool ShowWindow(int _instanceId, int line)
    {
        UnityEngine.Object item = EditorUtility.InstanceIDToObject(_instanceId);

        if (item is DialogueContainerSO)
        {
            DialogueEditorWindow window = (DialogueEditorWindow)GetWindow(typeof(DialogueEditorWindow));
            window.titleContent = new GUIContent("Dialogue Editor");
            window.currentDialogueContainer = item as DialogueContainerSO;
            window.minSize = new Vector2(750, 650);
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

        saveAndLoad = new DialogueSaveAndLoad(graphView);
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
        if (currentDialogueContainer != null)
        {
            Language(LanguageType.Russian, toolbarMenu);
            nameOfDialogueContainer.text = "Name: " + currentDialogueContainer;
            saveAndLoad.Load(currentDialogueContainer);
        }
    }

    private void Save()
    {
        if (currentDialogueContainer != null)
        {
            saveAndLoad.Save(currentDialogueContainer);
        }
    }

    private void Language(LanguageType _language, ToolbarMenu _toolbarMenu)
    {
        toolbarMenu.text = "Language: " + _language.ToString();
        languageType = _language;
        graphView.LanguageReload();
    }
}
