using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    [SerializeField] private string[] scenesPath;
    [SerializeField] private GameObject audioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(audioPlayer);
    }

    public void PressedStart(Dropdown dropdown)
    {
        SceneManager.LoadScene(scenesPath[dropdown.value]);
    }

    public void PressedExit()
    {
        Application.Quit();
    }
}
