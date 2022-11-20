using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Debug.Log("PauseMenu -> Game Paused");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        Debug.Log("PauseMenu -> Game Resumed");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Controls()
    {
        Debug.Log("PauseMenu -> Button Clicked -> Controls");
        controlsUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void Home(/*int sceneID*/)
    {
        Debug.Log("PauseMenu -> Button Clicked -> Home");
        Time.timeScale = 1f;
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        //SceneManager.LoadScene(sceneID);
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("PauseMenu -> Button Clicked -> Quit");
        Application.Quit();
    }
}
