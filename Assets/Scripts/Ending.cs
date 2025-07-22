using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public TMP_Text content;
    private PlayerManager _playerManager;

    private void Start()
    {
        _playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();

        if (_playerManager.goodMessengerTally > _playerManager.nosyMessengerTally && _playerManager.goodMessengerTally > _playerManager.greaterGoodMessengerTally) {
            content.text = "You have been a good messenger. You have delivered most if not all messages as they were intended. You have not altered most if not all messages in any way. You have not read most if not all messages. You have been a good messenger.";
        } else if (_playerManager.nosyMessengerTally > _playerManager.goodMessengerTally && _playerManager.nosyMessengerTally > _playerManager.greaterGoodMessengerTally) {
            content.text = "You have been a nosy messenger. You have read the messages. You have shared the messages. You have not delivered the messages as they were intended. But, you have altered most if not all messages in some way. You have been a nosy messenger.";
        } else if (_playerManager.greaterGoodMessengerTally > _playerManager.goodMessengerTally && _playerManager.greaterGoodMessengerTally > _playerManager.nosyMessengerTally) {
            content.text = "You have been a messenger for the greater good. You did not delivered the messages as they were intended. You went against the system. You changed the messages to benefit the citizens. You have been a messenger for the greater good.";
        }
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
}
