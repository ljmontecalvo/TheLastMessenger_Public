using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Start() {
        DontDestroyOnLoad(gameObject);

        if (GameObject.FindGameObjectsWithTag("Music").Length > 1) {
            Destroy(gameObject);
        }
    }
}
