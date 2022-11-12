using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
   public void PlayGame()
   {
        Debug.Log("StartMenu -> Button Clicked -> Play");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }

   public void QuitGame()
   {
        Debug.Log("StartMenu -> Button Clicked -> Quit");
        Application.Quit();
   }

   //public void Back()
   //{
        //SceneManager.LoadScene("Menu1");
   //}
}
