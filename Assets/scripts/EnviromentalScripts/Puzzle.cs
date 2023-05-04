using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles;
    [SerializeField] private Vector2[] locations;
    public int[] currentLoc;
    [SerializeField] private int openTile;
    [SerializeField] int[] keyTiles;
    private bool tileMoving = false;
    private int currentTile;
    public AudioSource AudioS;
    public AudioClip powerSound;
    public AudioClip dePowerSound;
    public AudioClip succesSound;
    public AudioClip cantDo;
    public bool isCompleted;
    void Start()
    {
        //Set the locations for each tile count
        locations = new Vector2[16];
        for (int i=0; i < tiles.Length; i++)
        {
            locations[i] = tiles[i].transform.position;
        }
        //For each tile move it to its puzzle spot and change the image if it is active
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].transform.position = locations[currentLoc[i]];
            if(currentLoc[i] != i) { 
                Color tmp = tiles[i].GetComponent<SpriteRenderer>().color;
                tmp.a = 0.0f;
                tiles[i].GetComponent<SpriteRenderer>().color = tmp;
            }
        }
        currentTile = currentLoc[openTile];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Click(int tileNumber)
    {
        //Debug.Log("Clicked on");
        int loc = currentLoc[tileNumber];
        if ((loc + 4 == currentTile || loc - 4 == currentTile || loc + 1 == currentTile || loc - 1 == currentTile) && !tileMoving)
        {
            StartCoroutine(Swap(tileNumber));
        }
    }
    public IEnumerator Swap(int tileNumber)
    {
        Color tmpC = tiles[tileNumber].GetComponent<SpriteRenderer>().color;
        tmpC.a = 0.0f;
        tiles[tileNumber].GetComponent<SpriteRenderer>().color = tmpC;
        bool isKey = false;
        for (int i = 0; i < keyTiles.Length; i++)
        {
            if (tileNumber == keyTiles[i])
            {
                isKey = true;
                break;
            }
        }
        if (isKey) { if (tileNumber == currentLoc[tileNumber]) { AudioS.PlayOneShot(dePowerSound); } }
        tileMoving = true;
        float timepass = 0f;
        timepass = Time.deltaTime;
        while((Vector2)tiles[tileNumber].transform.position != locations[currentTile])
        {
            timepass += Time.deltaTime;
            tiles[tileNumber].transform.position = Vector2.MoveTowards(locations[currentLoc[tileNumber]], locations[currentTile], timepass*4*4);
            yield return null;
        }
        int tmp = currentLoc[tileNumber];
        currentLoc[tileNumber] = currentTile;
        currentTile = tmp;
        tileMoving = false;
        if (isKey) { checkPosition(tileNumber); }
    }

    public void checkPosition(int tileNumber) 
    {
        isCompleted = true;
        if (tileNumber == currentLoc[tileNumber]) {
            Color tmp = tiles[tileNumber].GetComponent<SpriteRenderer>().color;
            tmp.a = 1.0f;
            tiles[tileNumber].GetComponent<SpriteRenderer>().color = tmp;
            for (int i = 0; i < keyTiles.Length; i++)
            {
                int tile = keyTiles[i];
                if(currentLoc[tile] != tile)
                {
                    isCompleted = false;
                }
            }
            if (isCompleted)
            {
                AudioS.PlayOneShot(succesSound);

            } else { AudioS.PlayOneShot(powerSound); }
        }
    }
}
