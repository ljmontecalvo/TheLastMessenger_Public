using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour {
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    
    private string _displayString = "";
    private string _displayNameString = "";
    private Image _backgroundImage;
    
    [HideInInspector] public bool printingText; // Flag to stop interactions with the NPC while the text is being printed.

    private void Start() {
        _backgroundImage = GetComponent<Image>();
        SetDialogueBoxVisibility(false);
    }

    private void Update() {
        dialogueText.text = _displayString;
        nameText.text = _displayNameString;
    }

    public void PrintText(string characterName, string text) {
        if (printingText) return;
        _displayNameString = characterName;
        _displayString = "";
        StartCoroutine(TimeText(text));
        SetDialogueBoxVisibility(true);
    }

    public void CloseBox() {
        _displayString = "";
        _displayNameString = "";
        SetDialogueBoxVisibility(false);
    }
    

    private void SetDialogueBoxVisibility(bool isVisible) {
        _backgroundImage.enabled = isVisible;
        dialogueText.enabled = isVisible;
        nameText.enabled = isVisible;
    }
    
    private IEnumerator TimeText(string text) {
        printingText = true;
        foreach (var t in text) {
            _displayString += t;
            yield return new WaitForSeconds(0.02f);
        }
        printingText = false;
    }
}
