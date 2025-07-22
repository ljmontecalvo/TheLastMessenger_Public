using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ActionsManager : MonoBehaviour {
    //Reference Variables
    public GameObject editButton1;
    public GameObject editButton2;
    
    public Animator infoBoxAnimator1;
    public Animator messageEditorAnimator1;
    public Animator messageAlterationAnimator1;
    
    public Animator infoBoxAnimator2;
    public Animator messageEditorAnimator2;
    public Animator messageAlterationAnimator2;

    // Letter Text Reference Variables
    public TMP_Text letter1Address;
    public TMP_Text letter1Content;
    public TMP_Text letter1Signature;
    public TMP_Text letter1AlterationAddress;
    public TMP_Text letter1AlterationContent;
    public TMP_Text letter1AlterationSignature;
    
    public TMP_Text letter2Address;
    public TMP_Text letter2Content;
    public TMP_Text letter2Signature;
    public TMP_Text letter2AlterationAddress;
    public TMP_Text letter2AlterationContent;
    public TMP_Text letter2AlterationSignature;

    public TMP_Text moneyText;

    public InventoryManager _inventoryManager;
    private PlayerManager _playerManager;

    private void Start() {
        _playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
    }

    private void Update() {
        // TODO: Fix this.
        if (editButton1 == null && editButton2 == null) { return; }

        if (_playerManager.letters.Count == 0) {
            editButton1.SetActive(false);
            editButton2.SetActive(false);
        } else if (_playerManager.letters.Count == 1) {
            editButton1.SetActive(true);
            editButton2.SetActive(false);
        } else if (_playerManager.letters.Count == 2) {
            editButton1.SetActive(true);
            editButton2.SetActive(true);
        }
    
        moneyText.text = $"${_playerManager.money}";
    }

    public void OpenActionsDialogue(int menuID) {
        if (menuID == 1) {
            infoBoxAnimator1.SetBool("active", true);
        } else if (menuID == 2) {
            infoBoxAnimator2.SetBool("active", true);
        }
    }

    public void CloseActionsDialogue() {
        infoBoxAnimator1.SetBool("active", false);
        infoBoxAnimator2.SetBool("active", false);
    }

    public void MessageActionsDialogue(int letterID) {
        if (letterID == 1) {
            _playerManager.letters[0].read = true;
            letter1Address.text = $"Dear, {_playerManager.letters[0].recipientName}";
            letter1Content.text = _playerManager.letters[0].content;
            letter1Signature.text = $"From, {_playerManager.letters[0].senderName}";
            
            letter1AlterationAddress.text = $"Dear, {_playerManager.letters[0].recipientName}";
            letter1AlterationContent.text = _playerManager.letters[0].tamperedContent;
            letter1AlterationSignature.text = $"From, {_playerManager.letters[0].senderName}";
            
            messageEditorAnimator1.SetBool("active", true);
            
            if (_playerManager.letters[0].tampered) {
                MessageAlterationActionsDialogue(1);
            }
        } else if (letterID == 2) {
            _playerManager.letters[1].read = true;
            letter2Address.text = $"Dear, {_playerManager.letters[1].recipientName}";
            letter2Content.text = _playerManager.letters[1].content;
            letter2Signature.text = $"From, {_playerManager.letters[1].senderName}";
            
            letter2AlterationAddress.text = $"Dear, {_playerManager.letters[1].recipientName}";
            letter2AlterationContent.text = _playerManager.letters[1].tamperedContent;
            letter2AlterationSignature.text = $"From, {_playerManager.letters[1].senderName}";
            
            messageEditorAnimator2.SetBool("active", true);

            if (_playerManager.letters[1].tampered) {
                MessageAlterationActionsDialogue(2);
            }
        }
    }

    public void MessageAlterationActionsDialogue(int letterID)
    {
        if (letterID == 1) {
            _playerManager.letters[0].tampered = true;
            messageAlterationAnimator1.SetBool("active", true);
        } else if (letterID == 2) {
            _playerManager.letters[1].tampered = true;
            messageAlterationAnimator2.SetBool("active", true);
        }
    }

    public void CloseAlterationActionsDialogue(int letterID)
    {
        if (letterID == 1) {
            _playerManager.letters[0].tampered = false;
            messageAlterationAnimator1.SetBool("active", false);
        } else if (letterID == 2) {
            _playerManager.letters[1].tampered = false;
            messageAlterationAnimator2.SetBool("active", false);
        }
    }

    public void CloseMessageEditor() {
        messageEditorAnimator1.SetBool("active", false);
        messageAlterationAnimator1.SetBool("active", false);
        
        messageEditorAnimator2.SetBool("active", false);
        messageAlterationAnimator2.SetBool("active", false);

        CloseActionsDialogue();
        _inventoryManager.HideInfo1();
        _inventoryManager.HideInfo2();
    }
}