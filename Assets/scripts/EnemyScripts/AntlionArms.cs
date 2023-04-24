using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntlionArms : MonoBehaviour
{
    [SerializeField] private LayerMask willAttack;
    [SerializeField] private Collider2D allertedZone;

    [SerializeField] private Vector3 restingPosition;
    [SerializeField] private float speed = 6;
    [SerializeField] private GameObject targeter;
    [SerializeField] private GameObject wrist;

    public List<GameObject> Enemies = new List<GameObject>();

    

    // Start is called before the first frame update
    void Start()
    {
        restingPosition = targeter.transform.position;
        StartCoroutine(Idle());
    }

    // Update is called once per frame
    void Update()
    {
        if(Enemies.Count < 1)
        {
            wrist.transform.rotation = Quaternion.AngleAxis(280, Vector3.forward);
        }
    }
    public IEnumerator Idle()
    {
        Vector3 lastPos;
        Vector3 newPos = restingPosition;
        
        //if an item enters the area, break'
        while (Enemies.Count < 1)
        {
            float timeSince = 0.0f;
            lastPos = targeter.transform.position;
            newPos.x = newPos.x + Random.Range(-1.0f, 1.0f);
            newPos.y = newPos.y + Random.Range(-1.0f, 1.0f);
            while (timeSince < 1 && Enemies.Count < 1)
            {
                timeSince += Time.deltaTime;
                targeter.transform.position = Vector3.Slerp(lastPos, newPos, timeSince);
                yield return null;
                
            }
        }
        StartCoroutine(Alerted());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Out");
        if (willAttack == collision.gameObject.layer)
        {
            Enemies.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == willAttack)
        {
            Enemies.Remove(collision.gameObject);
        }
    }
    public IEnumerator Alerted()
    {
        float timer = 4.0f;
        if(Enemies.Count > 0)
        {
            while (timer >= 0) { timer -= Time.deltaTime; yield return null; }
            StartCoroutine(Attacking());
        } 
        if ( Enemies.Count <= 0) { StartCoroutine(Idle()); }
    }
    public IEnumerator Attacking()
    {
        float timeSince = 0.0f;
        Vector3 lastPos = targeter.transform.position;
        Vector3 newPos = Enemies[0].transform.position;
        while (timeSince < 1)
        {
            timeSince += Time.deltaTime * speed;
            targeter.transform.position = Vector3.Slerp(lastPos, newPos, timeSince);
            yield return null;
        }
        StartCoroutine(Idle());
    }

    void Grappling()
    {

    }
    void Stunned()
    {

    }
    void Missed()
    {

    }
}
