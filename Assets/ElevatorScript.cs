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
    [SerializeField] int currentLevel = 0;
    private bool Moving = false;
    public Vector2 currentTarget;
    public float existance;
    
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
        if(!Moving)
            StartCoroutine(IUp());
    }
    public IEnumerator IUp()
    {
        Moving = true;
        existance = 0.0f;
        player.transform.position = tp.position;
        if(currentLevel == 0) 
        {
            Debug.Log("So Far So  Good");
            while (transform.position != levels[1])
            {
                existance = existance + Time.deltaTime;
                currentTarget = Vector2.Lerp(levels[0], levels[1], existance);
                transform.position = currentTarget;
                yield return null;
            }
            currentLevel = 1;
        }
        else if(currentLevel == 1)
        {
            if(puzzle1.isCompleted || puzzle1.isCompleted)
            {
                while(transform.position != levels[2])
                {
                    existance = existance + Time.deltaTime;
                    transform.position = Vector2.Lerp(levels[1], levels[2], existance / 2);
                    yield return null;
                }
            }
            currentLevel = 2;
        }
        else if (currentLevel == 2)
        {
            if(puzzle2.isCompleted && puzzle1.isCompleted)
            {
                while (transform.position != levels[3])
                {
                    existance = existance + Time.deltaTime;
                    transform.position = Vector2.Lerp(levels[2], levels[3], existance / 2);
                    yield return null;
                }
            }
            currentLevel = 3;
        }
        Moving = false;
    }
    public void Down()
    {
        if (!Moving)
            StartCoroutine(IDown());
    }
    public IEnumerator IDown()
    {
        Moving = true;
        float existance = 0.0f;
        if (currentLevel == 1)
        {
            while (transform.position != levels[0])
            {
                existance = existance + Time.deltaTime;
                transform.position = Vector2.Lerp(levels[1], levels[0], existance / 2);
                yield return null;
            }
            currentLevel = 0;
        }
        else if (currentLevel == 2)
        {

            while (transform.position != levels[1])
            {
                existance = existance + Time.deltaTime;
                transform.position = Vector2.Lerp(levels[2], levels[1], existance / 2);
                yield return null;
            }
            currentLevel = 1;

        } else if (currentLevel == 3)
        {
            while (transform.position != levels[2])
            {
                existance = existance + Time.deltaTime;
                transform.position = Vector2.Lerp(levels[3], levels[2], existance / 2);
                yield return null;
            }
            currentLevel = 2;
        }
        Moving = false;
    }
    
}
