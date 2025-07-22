using System;
using System.Collections;
using Cainos.CustomizablePixelCharacter;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class NPCv2 : MonoBehaviour {
    [Header("NPC Settings")]
    public string npcName;
    public bool startAlerted;

    [Header("Patrol Settings")]
    public bool enableMovement;
    public float movementSpeed;
    public float movementDuration;
    public float movementPauseDelay;

    [Header("Dialogue Settings")] 
    public bool enableDialogue;
    public bool oneAndDone;
    public int oneAndDoneDirection;
    public int dialogueSetCount;
    public List<string> dialogue1 = new();
    public List<string> dialogue2 = new();

    [Header("Prerequisite Options")] 
    public List<string> prerequisiteDialogue = new();
    [Header("Item Prerequiste")]
    public bool enableItemPrerequisite;
    public GameObject prerequisteItem;
    [Header("NPC Prerequisite")]
    public bool enableNPCPrerequisite;
    public NPCv2 prerequisteNPC;
    
    [Header("Letter Settings")]
    public bool enableLetter;
    public string transactionDialogue;
    public string title;
    public string objective;
    public string content;
    public string tamperedContent;
    public string senderName;
    public string receiverName;
    [Header("Receives Letter Settings")]
    public bool receivesLetter;
    public List<string> untouchedDialogue = new();
    public List<string> readDialogue = new();
    public List<string> alteredDialogue = new();
    public int payOut;
    
    [Header("Other")]
    public Animator alertIcon;
    
    // Reference Objects
    private PixelCharacterController _controller;
    private GameObject _player;
    private PlayerManager _playerManager;
    private GameObject _toolTip;
    
    // Accessable Reference Objects & Variables
    [HideInInspector] public DialogueBox dialogueBox;
    [HideInInspector] public PlayerManager.Letter receivingLetter;
     public int currentDialogueSet; // 0 - pre dialogue, -3 - untouched dialogue, -2 - read dialogue, -1 - altered dialogue
    [HideInInspector] public bool endingDialogueCycle;
    [HideInInspector] public bool hasSpoken;
    
    // In-Class Objects & Variables
    private Coroutine _patrolCoroutine;
    private bool _alerted;
    private bool _playerInRange;
    private bool _playerIsDeliveringLetter;

    private void Start() {
        _controller = GetComponent<PixelCharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player");
        dialogueBox = GameObject.FindGameObjectWithTag("Dialogue Box").GetComponent<DialogueBox>();
        _toolTip = GameObject.FindGameObjectWithTag("Tool Tip");
        _playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
        
        _controller.walkSpeedMax = movementSpeed;
        
        // Variable Error Handling
        if (enableNPCPrerequisite && enableItemPrerequisite) {
            enableDialogue = false;
            enableItemPrerequisite = false;
            Debug.LogError("Cannot select both NPC and item prerequisite options.");
        } else if (enableNPCPrerequisite || enableItemPrerequisite) {
            currentDialogueSet = 0;
            startAlerted = false;
        } else if (receivesLetter) {
            currentDialogueSet = 0;
        } else {
            currentDialogueSet = 1;
        }

        if (oneAndDone && dialogueSetCount < 1) {
            oneAndDone = false;
            Debug.LogError("Cannot have multiple dialogue sets and be one and done.");
        }
        
        // Normal Option Checks
        if (enableMovement) {
            _patrolCoroutine = StartCoroutine(Patrol());
        }

        if (startAlerted) {
            _alerted = true;
        }
        
        // Special Use Case Checks
        if (enableLetter) {
            dialogue1.Add(transactionDialogue);
        }
        
        if (oneAndDone) {
            dialogue1.Add("");
        }
    }

    // Update Method Flag Booleans
    private bool _prerequisiteMet;
    private bool _gaveLetter;
    private bool _tookLetter;
    private bool _startedExit;
    private void Update() {
        // Special Use Case Logic
        if (!_prerequisiteMet) {
            if (enableItemPrerequisite) {
                foreach (var item in _playerManager.objects) {
                    if (item == prerequisteItem) {
                        _prerequisiteMet = true;
                        currentDialogueSet = 1;
                    }
                }
            } else if (enableNPCPrerequisite) {
                if (prerequisteNPC.hasSpoken) {
                    enableDialogue = true;
                    _alerted = true;
                    currentDialogueSet = 1;
                    _prerequisiteMet = true;
                }
            }
        }

        if (!_gaveLetter) {
            if (enableLetter) {
                if (enableItemPrerequisite || enableNPCPrerequisite) {
                    if (_prerequisiteMet) {
                        if (endingDialogueCycle && !dialogueBox.printingText) {
                            _playerManager.letters.Add(new PlayerManager.Letter(title, objective, content, tamperedContent, senderName, receiverName));
                            _gaveLetter = true;
                        }
                    }
                } else {
                    if (endingDialogueCycle && !dialogueBox.printingText) {
                        _playerManager.letters.Add(new PlayerManager.Letter(title, objective, content, tamperedContent, senderName, receiverName));
                        _gaveLetter = true;
                    }
                }
            }
        }

        if (!_startedExit) {
            if (oneAndDone) {
                if (endingDialogueCycle && !dialogueBox.printingText) {
                    if (currentDialogueSet == dialogueSetCount) {
                        dialogueBox.CloseBox();
                        StartCoroutine(OneAndDone());
                        _startedExit = true;
                    }
                }
            }
        }
        
        // Normal Option Logic
        if (_alerted) {
            alertIcon.SetBool("alert", true);
        } else {
            alertIcon.SetBool("alert", false);
        }

        if (_playerInRange && Input.GetKeyDown(KeyCode.E) && !_startedExit && !dialogueBox.printingText) {
            // Talk to player.
            if (_patrolCoroutine != null) {
                StopCoroutine(_patrolCoroutine);
            }

            _controller.inputMove.x = 0f;

            if (receivesLetter && receivingLetter != null) {
                if (!_tookLetter) {
                    if (!dialogueBox.printingText) {
                        if (receivingLetter.tampered) {
                            currentDialogueSet = -1;
                            _playerManager.letters.Remove(receivingLetter);
                            _tookLetter = true;
                        } else if (receivingLetter.read) {
                            currentDialogueSet = -2;
                            _playerManager.letters.Remove(receivingLetter);
                            _tookLetter = true;
                        } else {
                            currentDialogueSet = -3;
                            _playerManager.letters.Remove(receivingLetter);
                            _tookLetter = true;
                        }
                        _playerManager.money += payOut;
                    }
                }
            }
            
            Speak();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!enableDialogue) { return; }
        if (!other.CompareTag("Sensor")) { return; }
        
        _playerInRange = true;
        _toolTip.GetComponent<TMP_Text>().enabled = true;
    }

    private bool _wasSpeaking;
    private void OnTriggerExit2D(Collider2D other) {
        if (!enableDialogue) { return; }
        if (!other.CompareTag("Sensor")) { return; }

        if (enableMovement && _wasSpeaking && !oneAndDone) {
            _patrolCoroutine = StartCoroutine(Patrol());
            _wasSpeaking = false;
        }

        _playerInRange = false;
        _toolTip.GetComponent<TMP_Text>().enabled = false;
        dialogueBox.CloseBox();
    }

    private IEnumerator Patrol() {
        _controller.inputMove.x = 1f;
        yield return new WaitForSeconds(movementDuration);
        _controller.inputMove.x = 0f;
        yield return new WaitForSeconds(movementPauseDelay);
        _controller.inputMove.x = -1f;
        yield return new WaitForSeconds(movementDuration);
        _controller.inputMove.x = 0f;
        yield return new WaitForSeconds(movementPauseDelay);
        _patrolCoroutine = StartCoroutine(Patrol());
    }

    private int _currentDialogueIndex;
    private void Speak() {
        _wasSpeaking = true;
        
        if (_alerted) {
            _alerted = false;
        }

        if (Vector2.Distance(transform.position, _player.transform.position) <= 0.8f) {
            StartCoroutine(DistanceFromPlayer());
        } else {
            FacePlayer();
        }

        switch (currentDialogueSet) {
            case 0 when _currentDialogueIndex == 0 && endingDialogueCycle:
                dialogueBox.CloseBox();
                endingDialogueCycle = false;
                break;
            case 0:
                dialogueBox.PrintText(npcName, prerequisiteDialogue[_currentDialogueIndex]);

                if (_currentDialogueIndex < prerequisiteDialogue.Count - 1) {
                    _currentDialogueIndex++;
                } else { 
                    _currentDialogueIndex = 0;
                    hasSpoken = true;
                    endingDialogueCycle = true;
                }

                break;
            case 1 when _currentDialogueIndex == 0 && endingDialogueCycle:
                dialogueBox.CloseBox();
                endingDialogueCycle = false;
                break;
            case 1:
                dialogueBox.PrintText(npcName, dialogue1[_currentDialogueIndex]);

                if (_currentDialogueIndex < dialogue1.Count - 1) {
                    _currentDialogueIndex++;
                } else {
                    _currentDialogueIndex = 0;
                    hasSpoken = true;
                    endingDialogueCycle = true;

                    if (dialogueSetCount > 1) {
                        currentDialogueSet++;
                    }
                }

                break;
            case 2 when _currentDialogueIndex == 0 && endingDialogueCycle:
                dialogueBox.CloseBox();
                endingDialogueCycle = false;
                break;
            case 2:
                dialogueBox.PrintText(npcName, dialogue2[_currentDialogueIndex]);

                if (_currentDialogueIndex < dialogue2.Count - 1) {
                    _currentDialogueIndex++;
                } else {
                    _currentDialogueIndex = 0;
                    hasSpoken = true;
                    endingDialogueCycle = true;
                }

                break;
            // Handle receiving letter dialogue:
            case -3 when _currentDialogueIndex == 0 && endingDialogueCycle:
                dialogueBox.CloseBox();
                endingDialogueCycle = false;
                _playerManager.goodMessengerTally++;
                currentDialogueSet = 2;
                break;
            case -3:
                dialogueBox.PrintText(npcName, untouchedDialogue[_currentDialogueIndex]);
                
                if (_currentDialogueIndex < untouchedDialogue.Count - 1) {
                    _currentDialogueIndex++;
                } else {
                    _currentDialogueIndex = 0;
                    hasSpoken = true;
                    endingDialogueCycle = true;
                }
                
                break;
            case -2 when _currentDialogueIndex == 0 && endingDialogueCycle:
                dialogueBox.CloseBox();
                endingDialogueCycle = false;
                _playerManager.nosyMessengerTally++;
                currentDialogueSet = 2;
                break;
            case -2:
                dialogueBox.PrintText(npcName, readDialogue[_currentDialogueIndex]);
                
                if (_currentDialogueIndex < readDialogue.Count - 1) {
                    _currentDialogueIndex++;
                } else {
                    _currentDialogueIndex = 0;
                    hasSpoken = true;
                    endingDialogueCycle = true;
                }
                
                break;
            case -1 when _currentDialogueIndex == 0 && endingDialogueCycle:
                dialogueBox.CloseBox();
                endingDialogueCycle = false;
                _playerManager.greaterGoodMessengerTally++;
                currentDialogueSet = 2;
                break;
            case -1:
                dialogueBox.PrintText(npcName, alteredDialogue[_currentDialogueIndex]);
                
                if (_currentDialogueIndex < alteredDialogue.Count - 1) {
                    _currentDialogueIndex++;
                }
                else {
                    _currentDialogueIndex = 0;
                    hasSpoken = true;
                    endingDialogueCycle = true;
                }
                
                break;
        }
    }

    private IEnumerator DistanceFromPlayer() {
        _controller.walkSpeedMax = 3f;
        
        if (_player.GetComponent<PixelCharacterController>().facingDir == -1) {
            _controller.inputMove.x = -1f;
        } else {
            _controller.inputMove.x = 1f;
        }
        
        yield return new WaitForSeconds(0.7f);
        _controller.inputMove.x = 0f;
        _controller.walkSpeedMax = movementSpeed;
        
        FacePlayer();
    }

    private void FacePlayer() {
        if (_player.transform.position.x < transform.position.x) {
            _controller.facingDir = -1;
        } else {
            _controller.facingDir = 1;
        }
    }
    
    // Special Use Case Methods
    private IEnumerator OneAndDone() {
        _wasSpeaking = false;
        
        _controller.facingDir = oneAndDoneDirection;  
        _controller.inputRun = true;
        _controller.runSpeedMax = 12f;
        _controller.inputMove.x = 1f * oneAndDoneDirection;
        
        yield return new WaitForSeconds(4f);
        
        Destroy(gameObject);
    }
}
