using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerPos : MonoBehaviour
{
    public GameMaster gm;
    // Start is called before the first frame update
    void Start()
    {
         gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
         transform.position = gm.lastCheckPointPos;
    }

    // Update is called once per frame
    
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }
}
