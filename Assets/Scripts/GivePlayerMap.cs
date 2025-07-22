using UnityEngine;

public class GivePlayerMap : MonoBehaviour
{
    private PlayerManager _playerManager;
    private NPCv2 _npc;

    private void Start()
    {
        _playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
        _npc = GetComponent<NPCv2>();
    }

    private bool _gaveMap;
    private void Update()
    {
        if (_npc.endingDialogueCycle && !_npc.dialogueBox.printingText && !_gaveMap)
        {
            _gaveMap = true;
            _playerManager.hasMap = true;
            _playerManager.mapActive = true;
            _playerManager.map.SetBool("active", true);
        }
    }
}
