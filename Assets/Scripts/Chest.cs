using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour {
    public List<GameObject> contents;
    public float itemForce;
    public Transform itemSpawnPoint;
    
    private TMP_Text _toolTip;
    private Cainos.PixelArtPlatformer_VillageProps.Chest _chestController;

    private bool _playerInRange;
    private bool _opened;

    private void Start() {
        _toolTip = GameObject.FindGameObjectWithTag("Tool Tip").GetComponent<TMP_Text>();
        _chestController = GetComponent<Cainos.PixelArtPlatformer_VillageProps.Chest>();
    }

    private void Update() {
        if (_playerInRange && !_opened) {
            if (Input.GetKeyDown(KeyCode.E)) {
                _chestController.Open();
                _opened = true;
                _toolTip.enabled = false;
                
                foreach (var item in contents) {
                    var thing = Instantiate(item, itemSpawnPoint.position, itemSpawnPoint.rotation);
                    var direction = Random.Range(-0.5f, 0.5f);
                    thing.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    thing.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction, itemForce), ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Sensor")) { return; }
        if (_opened) { return; }
        _toolTip.enabled = true;
        _playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!other.CompareTag("Sensor")) { return; }
        _toolTip.GetComponent<TMP_Text>().enabled = false;
        _playerInRange = false;
    }
}
