using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThanksForPlaying : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject thanksUI;
    public GameObject creditsUI;

    public void Credits()
    {
        Debug.Log("ThanksForPlaying -> Button Clicked -> Credits");
        creditsUI.SetActive(true);
        thanksUI.SetActive(false);
    }

    public void Home(/*int sceneID*/)
    {
        Debug.Log("ThanksForPlaying -> Button Clicked -> Home");
        Time.timeScale = 1f;
        GameIsPaused = false;
        thanksUI.SetActive(false);
        //SceneManager.LoadScene(sceneID);
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("ThanksForPlaying -> Button Clicked -> Quit");
        Application.Quit();
    }
}
