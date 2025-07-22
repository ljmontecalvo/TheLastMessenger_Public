using System;
using UnityEngine;

public class ManagerInitializer : MonoBehaviour {
    private void Start() {
        GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>().LocateVariableObjects();
        GameObject.FindGameObjectWithTag("NPC Manager").GetComponent<NPCManager>().RunCycle();
    }
}