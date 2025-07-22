using System;
using TMPro;
using UnityEngine;

public class PrerequisiteItem : MonoBehaviour {
    private PlayerManager _playerManager;
    private TMP_Text _toolTip;

    private void Start() {
        _playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
        _toolTip = GameObject.FindGameObjectWithTag("Tool Tip").GetComponent<TMP_Text>();
    }

    private void Update() {
        if (_playerInRange()) {
            _toolTip.text = "Press [E] to pick up item.";
            _toolTip.GetComponent<TMP_Text>().enabled = true;
            if (Input.GetKeyDown(KeyCode.E)) {
                _playerManager.objects.Add(gameObject);
                
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                _toolTip.GetComponent<TMP_Text>().enabled = false;
                _toolTip.text = "Press [E] to interact.";

                gameObject.GetComponent<PrerequisiteItem>().enabled = false;
            }
            
        }
    }

    private bool _playerInRange() {
        float distance = Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        return distance < 3f;
    }
}
