using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject main;
    public GameObject instructions;

    public void Play() {
        SceneManager.LoadScene(1);
    }

    public void Instructions() {
        main.SetActive(false);
        instructions.SetActive(true);
    }

    public void Quit() {
        Application.Quit();
    }

    public void Back() {
        main.SetActive(true);
        instructions.SetActive(false);
    }
}
