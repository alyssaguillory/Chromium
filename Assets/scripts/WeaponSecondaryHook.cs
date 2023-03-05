using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSecondaryHook : WeaponBase
{
    public GameObject player;
    public float maxDist;
    public float spacing;
    public GameObject node;
    public float climbSpeed;
    public Vector3 target;
    public bool done;
    public float throwSpeed;
    public bool isActive;
    RaycastHit2D hit;
    public LayerMask whatIsGround;
    private Vector2 direction;
    public float MaxRange = 10;
    IEnumerator CurrentPath;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (CurrentPath == null)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
                    hit = Physics2D.Raycast(transform.position, direction, MaxRange, whatIsGround);
                    if (hit.collider != null) { CurrentPath = ThrowHit(); }
                    else { CurrentPath = Throw(); }
                    StartCoroutine(CurrentPath);
                }
            }
        }
    }
    
    //Change to coroutine
    IEnumerator ThrowHit()
    {
        while ((Vector2)transform.position != hit.point)
        {
            transform.position = Vector2.MoveTowards(transform.position, hit.point, speed);
            yield return null;
        }

        //Create Nodes

        //if w increase distances
        //if s decrease distances
        //if right click is clicked, quickly launch player
        //if at any point the distance is to short or the player lets go of the right click after clicking delete nodes and retrieve
    }

    IEnumerator Throw()
    {
        while ((Vector2)transform.position != hit.point)
        {
            transform.position = Vector2.MoveTowards(transform.position, hit.point, speed);
            yield return null;
        }
        while (transform.position != player.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
            yield return null;
        }
        yield return null;
    }

    void MoveTowards()
    {
        //Nodes[Nodes.Count - 1].GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
        //Vector2 normalToPlayer = (Nodes[Nodes.Count - 1].GetComponent<HingeJoint2D>().connectedAnchor).normalized;
        //Nodes[Nodes.Count - 1].GetComponent<HingeJoint2D>().connectedAnchor = Nodes[Nodes.Count - 1].GetComponent<HingeJoint2D>().connectedAnchor - (normalToPlayer * climbSpeed);
        //player.transform.position = Nodes[Nodes.Count - 1].transform.position;
    }

    void Retract()
    {

    }

    void Reel()
    {

    }
    //On left click, throw hook
    //When hook hits something, stop it
    //Hook gets frozen
    //Activate the hinge joint on it
    //Instantiate Nodes
    //Disable Autoconnect
    //Set Anchor to 0,0
    //Hook to node 1
    //NodeN to NodeN+1
    //NodeZ to player
    //Set Conected anchor to Dist,0
    //Set Position to (?,?)
    //If it misses retract

    //While Active, if holding leftclick, reel in towards it
    //Upon release, retract back to you

    //Retract
    //Destroy nodes and move hook directly back to you

    //Reel
    //Reduce Conected anchor

    public override void Deactivate()
    {
        isActive = false;
    }
    public override void Activate()
    {
        isActive = true;
    }
}
