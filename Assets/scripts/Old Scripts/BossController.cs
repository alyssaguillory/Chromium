using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject[] FireWalls;
    public GameObject[] Warnings;
    public GameObject BrokenPanel;
    public GameObject Laser;
    public GameObject LaserPrefab;
    public GameObject Conveyor;
    [SerializeField] GameObject wall;
    public int FireCount = 4;
    int gameStage = 0;
    float gameMultiplier = 1f;
    int times = 0;

    int Set = 0;
    public float FireInterval = 9;
    float TimeRemaining = 0;
    bool warned = false;
    bool fired = false;
    List<IEnumerator> blasts = new();
    IEnumerator IntervalSetter;
    IEnumerator laserer;
    // Start is called before the first frame update
    void Start()
    {
        blasts.Add(FirePlatform(0, FireInterval * gameMultiplier));
        blasts.Add(FirePlatform(0, FireInterval * gameMultiplier));
        blasts.Add(FirePlatform(0, FireInterval * gameMultiplier));
        blasts.Add(FirePlatform(0, FireInterval * gameMultiplier));
        blasts.Add(FirePlatform(0, FireInterval * gameMultiplier));
        blasts.Add(FirePlatform(0, FireInterval * gameMultiplier));
        blasts.Add(FirePlatform(0, FireInterval * gameMultiplier));
        blasts.Add(FirePlatform(0, FireInterval * gameMultiplier));
        IntervalSetter = Interval(FireInterval * gameMultiplier);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        /*
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
        */
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
    public void advanceGameStage()
    {
        gameStage++;
        switch (gameStage)
        {
            case 1:
                Instantiate(LaserPrefab, Laser.transform);
                laserer = laserFiring(2.0f);
                StartCoroutine(laserer);
                break;
            case 2:
                StopCoroutine(laserer);
                laserer = laserFiring(2.0f);
                Instantiate(LaserPrefab, Laser.transform).transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                Laser.transform.GetChild(0).transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                gameMultiplier = 0.9f;
                StartCoroutine(laserer);
                break;
            case 3:
                StopCoroutine(laserer);
                laserer = laserFiring(2.0f);
                Instantiate(LaserPrefab, Laser.transform).transform.eulerAngles = new Vector3(0.0f, 0.0f, 120.0f);
                Laser.transform.GetChild(0).transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                Laser.transform.GetChild(1).transform.eulerAngles = new Vector3(0.0f, 0.0f, 240.0f);
                gameMultiplier = 0.7f;
                StartCoroutine(laserer);
                break;
            case 4:
                StopCoroutine(laserer);
                laserer = laserFiring(2.0f);
                Instantiate(LaserPrefab, Laser.transform).transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
                Laser.transform.GetChild(0).transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                Laser.transform.GetChild(1).transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                Laser.transform.GetChild(2).transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                gameMultiplier = 0.5f;
                StartCoroutine(laserer);
                break;
            case 5: //Dead Stage
                stopBlasts();
                StartCoroutine(wallRaise());
                times = 100;
                break;
        }
        stopBlasts();
        if (IntervalSetter != null) { StopCoroutine(IntervalSetter); }
        IntervalSetter = Interval(FireInterval * gameMultiplier);
        if (gameStage != 5) { StartCoroutine(IntervalSetter); }
    }
    void  choosePlatforms()
    {
        int x = Random.Range(0,8);
        int y = x;
        //int z = x;
        while(x == y) { y = Random.Range(0, 8); }
        //while (z == y || z == x) { z = Random.Range(0, 8); }
        blasts[x] = FirePlatform(x, FireInterval * gameMultiplier);
        blasts[y] = FirePlatform(y, FireInterval * gameMultiplier);
        //blasts[z] = FirePlatform(z, FireInterval * gameMultiplier);
        StartCoroutine(blasts[x]);
        StartCoroutine(blasts[y]);
        //StartCoroutine(blasts[z]);
    }
    IEnumerator FirePlatform(int Column, float time)
    {
        Warnings[Column].GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(2.5f/9.0f * time);
        FireWalls[Column].GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(6.5f / 9.0f * time);
    }
    void stopBlasts()
    {
        for (int i = blasts.Count - 1; i <= 0; i++)
        {
            if (blasts[i] != null) { StopCoroutine(blasts[i]);}
        }
    }
    IEnumerator Interval(float time)
    {
        int counter = 0;
        while(gameStage != 5)
        {
            counter++;
            choosePlatforms();
            if(counter == 3)
            {
                if (laserer != null) { StopCoroutine(laserer); }
                laserer = laserFiring(2.0f);
                StartCoroutine(laserer);
                counter = 0;
            }
            yield return new WaitForSeconds(time + Random.Range(0.0f,1.0f));
        }
        yield return null;
    }
    IEnumerator laserTimer(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        warned = true;
    }
    IEnumerator laserFiring(float time)
    {
        float rotspeed = 40.0f;
        
        int dice;
        times = 0;
        IEnumerator laserlaser;
        ParticleSystem[] system = Laser.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem a in system)
        {
            a.Play();
        }

        while (rotspeed != 0)
        {
            warned = false;
            dice = Random.Range(0, 10);
            dice += times;
            times++;
            laserlaser = laserTimer(time);
            //Debug.Log(dice);
            if (dice < 3) { rotspeed = rotspeed * -1; StartCoroutine(laserlaser); }
            else if (dice < 7) { StartCoroutine(laserlaser); }
            else if (dice < 10) { rotspeed = rotspeed * 3; laserlaser = laserTimer(time * 0.25f); StartCoroutine(laserlaser); }
            else
            {
                rotspeed = 0;
                foreach (ParticleSystem a in system)
                {
                    a.Stop();
                }
                //choosePlatforms();
            }
            //Debug.Log(rotspeed);
            while (warned == false)
            {
                Laser.transform.Rotate(new Vector3(0.0f, 0.0f, rotspeed * Time.deltaTime));
                yield return null;
            }
            if (dice > 6) { rotspeed = rotspeed / 3; }
        }
    }

    IEnumerator wallRaise()
    {
        //7.12
        while (wall.transform.localPosition.y < 12.05)
        {
            wall.transform.localPosition = wall.transform.localPosition + new Vector3(0.0f, 15.0f, 0.0f) * Time.deltaTime;
            yield return null;
        }

    }
}
/* The boss should wait until the player passes a threshold, then activate
 * The boss should have a health system on its panels
 * To begin with, the boss should fire every 2x (2x) the interval and every 4x the interval make laser that follows the enemy.
 * The Panels, upon dying, activate a function in BossController.
 * This function immediately stops the laser and the other particles
 *      This function also decreases the interval to fire to 1.5x (2x), 1x(2x), 0.5x(2x)
 *      This function also increases the number of lasers that are fired to (2), (3), (4)
 *      The damage is increased by (1.5x), (1.7x), (2x)
 *      This function also immediately fires the laser and platforms
 * 
 */