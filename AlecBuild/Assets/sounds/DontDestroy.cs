using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

     private void Awake()
 {
     int numMusicPlayers = FindObjectsOfType<DontDestroy>().Length;
     if (numMusicPlayers != 1)
     {
         Destroy(this.gameObject);
     }
     // if more then one music player is in the scene
     //destroy ourselves
     else
     {
         DontDestroyOnLoad(gameObject);
     }
    
 }
}
