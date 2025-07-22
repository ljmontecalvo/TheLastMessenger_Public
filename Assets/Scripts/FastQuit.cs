using UnityEngine;

public class FastQuit : MonoBehaviour
{
    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
