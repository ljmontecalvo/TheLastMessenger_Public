using System;
using UnityEngine;

public class NPCSorter : MonoBehaviour
{
    private void Start()
    {
        var npcs = GameObject.FindGameObjectsWithTag("Background NPC");

        var index = 0;
        foreach (var npc in npcs)
        {
            var transform = npc.transform.position;
            npc.transform.position = new Vector3(transform.x, transform.y, transform.z - index);
            index++;
        }
    }
}
