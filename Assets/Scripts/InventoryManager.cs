using System;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    [Header("Boxes")]
    public GameObject info1;
    public GameObject info2;
    
    public TMP_Text info1Title;
    public TMP_Text info1Objective;
    
    public TMP_Text info2Title;
    public TMP_Text info2Objective;
    
    private PlayerManager _playerManager;
    
    private bool _info1Active;
    private bool _info2Active;

    private void Start() {
        _playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
    }

    private void Update() {
        info1Title.text = "No Active Message!";
        info2Title.text = "No Active Message!";
        
        info1Objective.text = "Your message will appear here.";
        info2Objective.text = "Your message will appear here.";
        
        if (_playerManager.letters.Count == 0) return;
        info1Title.text = _playerManager.letters[0].title;
        info1Objective.text = _playerManager.letters[0].objective;
        
        if (_playerManager.letters.Count == 1) return;
        info2Title.text = _playerManager.letters[1].title;
        info2Objective.text = _playerManager.letters[1].objective;
    }

    public void Info1() {
        if (_info1Active) {
            HideInfo1();
        } else {
            if (_info2Active) {
                HideInfo2();
            }
            ShowInfo1();
        }
    }

    public void Info2() {
        if (_info2Active) {
            HideInfo2();
        } else {
            if (_info1Active) {
                HideInfo1();
            }
            ShowInfo2();
        }
    }

    public void ShowInfo1() {
        info1.GetComponent<Animator>().SetBool("active", true);
        _info1Active = true;
    }
    
    public void HideInfo1() {
        info1.GetComponent<Animator>().SetBool("active", false);
        _info1Active = false;
    }

    public void ShowInfo2() {
        info2.GetComponent<Animator>().SetBool("active", true);
        _info2Active = true;
    }

    public void HideInfo2() {
        info2.GetComponent<Animator>().SetBool("active", false);
        _info2Active = false;
    }
}
