using System.Collections.Generic;
using Cainos.CustomizablePixelCharacter;
using TMPro;
using UnityEngine;

public class ShopKeeper : MonoBehaviour {
    [Header("NPC Settings")]
    public string npcName;
    public GameObject product;
    public bool startAlerted;

    [Header("Dialogue Settings")]
    public bool enableDialogue;

    [Header("Other")]
    public Animator alertIcon;
    
    // Reference Objects
    private PixelCharacterController _controller;
    private GameObject _player;
    private PlayerManager _playerManager;
    private GameObject _toolTip;
    
    // Accessable Reference Objects & Variables
    [HideInInspector] public DialogueBox dialogueBox;
    [HideInInspector] public int currentDialogueSet;
    
    // In-Class Objects & Variables
    private bool _alerted;
    private bool _playerInRange;

    private void Start() {
        _controller = GetComponent<PixelCharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player");
        dialogueBox = GameObject.FindGameObjectWithTag("Dialogue Box").GetComponent<DialogueBox>();
        _toolTip = GameObject.FindGameObjectWithTag("Tool Tip");
        _playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
    }

    // Update Method Flag Booleans
    private bool _startedExit;
    private void Update() {
        // Normal Option Logic
        if (_alerted) {
            alertIcon.SetBool("alert", true);
        } else {
            alertIcon.SetBool("alert", false);
        }

        if (_playerInRange && Input.GetKeyDown(KeyCode.E) && !_startedExit && !dialogueBox.printingText) {
            // Talk to player.
            _controller.inputMove.x = 0f;
            
            if (enableDialogue) {
                Speak();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Sensor")) { return; }
        
        _playerInRange = true;
        _toolTip.GetComponent<TMP_Text>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!other.CompareTag("Sensor")) { return; }

        _playerInRange = false;
        _toolTip.GetComponent<TMP_Text>().enabled = false;
        dialogueBox.CloseBox();
    }

    private bool _dialogueReset;
    private void Speak() {
        if (_alerted) {
            _alerted = false;
        }

        FacePlayer();

        string productName = product.GetComponent<ShopProduct>().name;
        int productPrice = product.GetComponent<ShopProduct>().price;

        if (currentDialogueSet == 0) {
            dialogueBox.PrintText(npcName, $"Hi, would you like to buy this {productName} for ${productPrice}?");
            
            if (_playerManager.money >= productPrice) {
                currentDialogueSet = 1;
            } else {
                currentDialogueSet = 2;
            }
        } else if (currentDialogueSet == 1 && _dialogueReset) {
            dialogueBox.CloseBox();
            enableDialogue = false;
            _dialogueReset = false;
        } else if (currentDialogueSet == 1) {
            dialogueBox.PrintText(npcName, $"Pleasure doing business! Enjoy your {productName}.");

            GivePlayerProduct(productPrice);
            _dialogueReset = true;
        } else if (currentDialogueSet == 2 && _dialogueReset) {
            dialogueBox.CloseBox();
            currentDialogueSet = 0;
            _dialogueReset = false;
        } else if (currentDialogueSet == 2) {
            Debug.Log("2");
            dialogueBox.PrintText(npcName, $"You don't have enough money! Come back when you have ${productPrice}!");

            _dialogueReset = true;
        }
    }

    private void FacePlayer() {
        if (_player.transform.position.x < transform.position.x) {
            _controller.facingDir = -1;
        } else {
            _controller.facingDir = 1;
        }
    }

    private void GivePlayerProduct(int productPrice) {
        _playerManager.objects.Add(product);
        _playerManager.money -= productPrice;
    }
}
