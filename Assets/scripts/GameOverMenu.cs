using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenuUI;

    void FixedUpdate()
    {
        
    }

    public void ReturnToCheckpoint()
    {

    }

    public void Home(/*int sceneID*/)
    {
        //SceneManager.LoadScene(sceneID);
        SceneManager.LoadScene(0);
        gameOverMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
