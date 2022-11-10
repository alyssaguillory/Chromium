using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenuUI;

    //void Update()
    //{

    //}

    public void ReturnToCheckpoint()
    {
        Debug.Log("GameOverMenu -> Button Clicked -> ReturnToCheckpoint");
    }

    public void Home(/*int sceneID*/)
    {
        Debug.Log("GameOverMenu -> Button Clicked -> Home");
        //SceneManager.LoadScene(sceneID);
        SceneManager.LoadScene(0);
        gameOverMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("GameOverMenu -> Button Clicked -> Quit");
        Application.Quit();
    }
}