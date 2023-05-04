using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    [SerializeField] Puzzle puzzle1;
    [SerializeField] Puzzle puzzle2;
    [SerializeField] Collider2D[] invisibleWalls;
    [SerializeField] HealthPickup locked;
    [SerializeField] SpriteRenderer padlock;
    [SerializeField] GameObject player;
    [SerializeField] Transform tp;
    [SerializeField] Vector3[] levels;
    [SerializeField] int currentLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(puzzle1.isCompleted && puzzle2.isCompleted)
        {
            locked.enabled = true;
            padlock.enabled = false;
        }
    }
    public void Up()
    {
        StartCoroutine(IUp());
    }
    public IEnumerator IUp()
    {
        float existance = 0.0f;
        player.transform.position = tp.position;
        if(transform.position == levels[0]) 
        {
            while (transform.position != levels[1])
            {
                existance += Time.deltaTime;
                transform.position = Vector3.MoveTowards(levels[0], levels[1], existance / 2);
                yield return null;
            }
        }
        else if(transform.position == levels[1])
        {
            if(puzzle1.isCompleted || puzzle1.isCompleted)
            {
                while(transform.position != levels[2])
                {
                    existance += Time.deltaTime;
                    transform.position = Vector3.MoveTowards(levels[1], levels[2], existance / 2);
                    yield return null;
                }
            }
        }
        else if (transform.position == levels[2])
        {
            if(puzzle2.isCompleted && puzzle1.isCompleted)
            {
                while (transform.position != levels[3])
                {
                    existance += Time.deltaTime;
                    transform.position = Vector3.MoveTowards(levels[2], levels[3], existance / 2);
                    yield return null;
                }
            }
        }
        
    }
    public void Down()
    {
        StartCoroutine(IDown());
    }
    public IEnumerator IDown()
    {
        float existance = 0.0f;
        if (transform.position == levels[1])
        {
            while (transform.position != levels[0])
            {
                existance += Time.deltaTime;
                transform.position = Vector3.MoveTowards(levels[1], levels[0], existance / 2);
                yield return null;
            }
        }
        else if (transform.position == levels[2])
        {

            while (transform.position != levels[1])
            {
                existance += Time.deltaTime;
                transform.position = Vector3.MoveTowards(levels[2], levels[1], existance / 2);
                yield return null;
            }
            
        } else if (transform.position == levels[3])
        {
            while (transform.position != levels[2])
            {
                existance += Time.deltaTime;
                transform.position = Vector3.MoveTowards(levels[3], levels[2], existance / 2);
                yield return null;
            }
        }
        yield return null;
    }

}
