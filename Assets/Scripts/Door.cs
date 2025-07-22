using TMPro;
using UnityEngine;

public class Door : MonoBehaviour {
    public GameObject key;
    public bool searchForKey;
    public bool npcPrerequisite;
    public NPCv2 npc;
    
    private PlayerManager _playerManager;
    private TMP_Text _toolTip;
    private Cainos.PixelArtPlatformer_Dungeon.Door _doorController;

    private bool _foundKey;
    private bool _playerInRange;
    private bool _opened;

    private void Start() {
        _playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
        _toolTip = GameObject.FindGameObjectWithTag("Tool Tip").GetComponent<TMP_Text>();
        _doorController = GetComponent<Cainos.PixelArtPlatformer_Dungeon.Door>();

        if (searchForKey) {
            if (GameObject.FindGameObjectWithTag("Key") != null) {
                key = GameObject.FindGameObjectWithTag("Key");
                _opened = true;
                _foundKey = true;
            }
        }
    }

    private void Update() {
        if (searchForKey && !_foundKey) {
            if (GameObject.FindGameObjectWithTag("Key") != null) {
                key = GameObject.FindGameObjectWithTag("Key");
                _foundKey = true;
            }
        }

        if (npcPrerequisite && !_opened) {
            if (npc.hasSpoken) {
                _opened = true;
                _doorController.Open();
            }
        }
        
        if (_playerInRange && !_opened && !npcPrerequisite) {
            if (Input.GetKeyDown(KeyCode.E)) {
                foreach (var item in _playerManager.objects) {
                    if (item == key) {
                        _doorController.Open();
                        _toolTip.enabled = false;
                        _opened = true;
                    }
                }

                _toolTip.text = "This door is locked!";
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Sensor")) { return; }
        if (_opened) { return; }
        if (npcPrerequisite) { return; }

        _toolTip.text = "Press [E] to open.";
        _toolTip.enabled = true;
        _playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!other.CompareTag("Sensor")) { return; }
        if (npcPrerequisite) { return; }
        _toolTip.enabled = false;
        _toolTip.text = "Press [E] to interact.";
        _playerInRange = false;
    }
}
