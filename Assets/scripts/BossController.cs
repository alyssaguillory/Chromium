using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject[] FireWalls;
    public GameObject[] Warnings;
    public int FireCount = 4;

    //Random rnd = new Random();
    int Set = 0;
    public float FireInterval = 9;
    float TimeRemaining = 0;
    bool warned = false;
    bool fired = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeRemaining > 0)
        {
            switch (Set)
            {
                case 0:
                    //fire again in three seconds
                    //TimeRemaining = 3;
                    break;
                case 1:
                    //fire [0] and [1]
                    Fire(0, 1);
                    break;
                case 2:
                    //fire [0] and [2]
                    Fire(0, 2);
                    break;
                case 3:
                    //fire [0] and [3]
                    Fire(0, 3);
                    break;
                case 4:
                    //fire [1] and [2]
                    Fire(1, 2);
                    break;
                case 5:
                    //fire [1] and [3]
                    Fire(1, 3);
                    break;
                case 6:
                    //fire [2] and [3]
                    Fire(2, 3);
                    break;
            }
            TimeRemaining -= Time.deltaTime;
        }
        else { ChooseNextFires(); }
        
    }

    void ChooseNextFires()
    {
        for(int i = 0; i< FireCount; i++)
        {
                Set = Random.Range(0, 6);
        }
        warned = false;
        fired = false;
        TimeRemaining = FireInterval;
        if (Set == 0) { TimeRemaining = 0.5f; }
    }

    void Fire(int FirstColumn, int SecondColumn)
    {
        if (!warned)
        {
            Warnings[FirstColumn].GetComponent<ParticleSystem>().Play();
            Warnings[SecondColumn].GetComponent<ParticleSystem>().Play();
            warned = true;
        } else if(!fired && TimeRemaining < FireInterval-2.5f)
        {
            FireWalls[FirstColumn].GetComponent<ParticleSystem>().Play();
            FireWalls[SecondColumn].GetComponent<ParticleSystem>().Play();
            fired = true;
        }
    }
}
