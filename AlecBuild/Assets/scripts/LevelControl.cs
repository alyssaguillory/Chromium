using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour
{
    public int index;
    public string levelName;
    public bool been_there_flag = false; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            //SceneManager.LoadScene(1);
            SceneManager.LoadScene(levelName);

        }
        /*if(other.CompareTag("Player") && been_there_flag == true)
        {
            //SceneManager.LoadScene(1);
            SceneManager.LoadScene(levelName+"-");

        } else if(other.CompareTag("Player") && been_there_flag == false) {

            SceneManager.LoadScene(levelName);

        }*/
    }
}
