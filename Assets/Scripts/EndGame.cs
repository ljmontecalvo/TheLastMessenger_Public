using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {
    private void Update() {
        if (GetComponent<NPCv2>().endingDialogueCycle) {
            End();
        }
    }

    public void End() {
        SceneManager.LoadScene(4);
    }
}
