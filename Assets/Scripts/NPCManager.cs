using System;
using UnityEngine;

public class NPCManager : MonoBehaviour {
    public static NPCManager Instance;
    
    public PlayerManager _playerManager;
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        _playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
        
        RunCycle();
    }

    public void RunCycle() {
        NPCv2[] npcs = GameObject.FindObjectsOfType<NPCv2>();

        if (_playerManager.letters.Count != 0) {
            foreach (var npc in npcs) {
                foreach (var letter in _playerManager.letters) {
                    if (letter.recipientName == npc.npcName) {
                        Debug.Log($"{letter.title} is for the NPC named {npc.npcName} in this scene!");
                        npc.receivingLetter = letter;
                    }
                }
            }
        }
    }
}