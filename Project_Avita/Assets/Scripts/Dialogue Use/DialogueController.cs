using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI;
    [Header("Text")]
    [SerializeField] private Text textName;
    [SerializeField] private Text textBox;
    [Header("Image")]
    [SerializeField] private Sprite centerImageGO;
    [Header("Buttons")]
    [SerializeField] private Button button01;
    [SerializeField] private Text buttonText01;
    [Space]
    [SerializeField] private Button button02;
    [SerializeField] private Text buttonText02;
    [Space]
    [SerializeField] private Button button03;
    [SerializeField] private Text buttonText03;
    [Space]
    [SerializeField] private Button button04;
    [SerializeField] private Text buttonText04;

    private List<Button> buttons = new List<Button>();
    private List<Text> buttonsTexts = new List<Text>();

    private void Awake()
    {
        ShowDialogue(false);

        buttons.Add(button01);
        buttons.Add(button02);
        buttons.Add(button03);
        buttons.Add(button04);

        buttonsTexts.Add(buttonText01);
        buttonsTexts.Add(buttonText02);
        buttonsTexts.Add(buttonText03);
        buttonsTexts.Add(buttonText04);
    }

    public void ShowDialogue(bool _show)
    {
        dialogueUI.SetActive(_show);
    }

    public void SetText(string _name, string _textBox)
    {
        textName.text = _name;
        textBox.text = _textBox;
    }

    public void SetImage(Sprite _image)
    {
        centerImageGO = _image;
    }

    public void SetButtons(List<string> _texts, List<UnityAction> _unityActions)
    {
        buttons.ForEach(button => button.gameObject.SetActive(false));

        for (int i = 0; i < _texts.Count; i++)
        {
            buttonsTexts[i].text = _texts[i];
            buttons[i].gameObject.SetActive(true);
            buttons[i].onClick = new Button.ButtonClickedEvent();
            buttons[i].onClick.AddListener(_unityActions[i]);
        }
    }
}
