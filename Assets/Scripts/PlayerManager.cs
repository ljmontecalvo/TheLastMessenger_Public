using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour {
    public class Letter {
        public string title;
        public string objective;
        public string content;
        public string tamperedContent;

        public string senderName;
        public string recipientName;
        
        public bool read;
        public bool tampered;
        
        public Letter(string title, string objective, string content, string tamperedContent, string senderName, string recipientName) {
            this.title = title;
            this.objective = objective;
            this.content = content;
            this.tamperedContent = tamperedContent;
            this.senderName = senderName;
            this.recipientName = recipientName;
        }
    }

    private static PlayerManager Instance;
    
    [Header("Invetory Settings")]
    public List<Letter> letters = new List<Letter>();
    public GameObject _inventoryIcon1;
    public GameObject _inventoryIcon2;
    public int money;
    
    [Header("Map Settings")]
    public bool hasMap;
    [HideInInspector] public Animator map;
    
    [Header("Prerequisites/Objects")]
    public List<GameObject> objects;

    [Header("Messenger Status")]
    public int goodMessengerTally;
    public int nosyMessengerTally;
    public int greaterGoodMessengerTally;

    [HideInInspector] public bool mapActive;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        LocateVariableObjects();
    }

    public void LocateVariableObjects() {
        _inventoryIcon1 = GameObject.FindGameObjectWithTag("Icon1");
        _inventoryIcon2 = GameObject.FindGameObjectWithTag("Icon2");
        
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<Animator>();

        mapActive = false;
    }

    private void Update() {
        switch (letters.Count) {
            case 0:
                _inventoryIcon1.GetComponent<Animator>().SetBool("active", false);
                _inventoryIcon2.SetActive(false);
                break;
            case 1:
                _inventoryIcon1.GetComponent<Animator>().SetBool("active", true);
                _inventoryIcon2.SetActive(false);
                break;
            case 2:
                _inventoryIcon1.GetComponent<Animator>().SetBool("active", true);
                _inventoryIcon2.SetActive(true);
                break;
        }

        if (Input.GetKeyDown(KeyCode.M) && hasMap && !mapActive) {
            map.SetBool("active", true);
            mapActive = true;
        } else if (Input.GetKeyDown(KeyCode.M) && hasMap && mapActive) {
            map.SetBool("active", false);
            mapActive = false;
        }
    }
}
