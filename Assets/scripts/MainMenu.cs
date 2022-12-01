using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainMenuUI;
    public GameObject controlsUI;
    public GameObject creditsUI;
    public GameMaster gm;

    public void PlayGame()
    {
        Debug.Log("StartMenu -> Button Clicked -> Play");
        
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        gm.lastCheckPointPos = new Vector2(55.5f, 14.4f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void Controls()
    {
        Debug.Log("StartMenu -> Button Clicked -> Controls");
        controlsUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void Credits()
    {
        Debug.Log("StartMenu -> Button Clicked -> Credits");
        creditsUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("StartMenu -> Button Clicked -> Quit");
        Application.Quit();
    }
}
