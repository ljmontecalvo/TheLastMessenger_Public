using UnityEngine;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour {
    public void ToCastle() {
        Debug.Log("To Castle");
        SceneManager.LoadScene(2);
    }

    public void ToRollingMeadows() {
        Debug.Log("To Rolling Meadows");
        SceneManager.LoadScene(3);
    }
}