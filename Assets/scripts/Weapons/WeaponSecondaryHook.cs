using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSecondaryHook : WeaponBase
{
    public float maxRange = 60, throwSpeed = 3, thrownForce = 300f;
    public float moveVelocity = 300, angleAllowance = 45, fullForceAngle=30;
    public int nodeCount = 5;
    
    public Vector3 target;
    public Vector2 pushing;

    [SerializeField] GameObject player;
    [SerializeField] GameObject baseNode;
    [SerializeField] GameObject node;
    [SerializeField] float climbSpeed;

    public List<GameObject> Nodes = new List<GameObject>();


    private LineRenderer lr;
    
    [SerializeField] GameObject hookNode;
    private Vector3 defaultHook;
    [SerializeField] GameObject shoulderNode;
    private Vector3 defaultShoulder;
    public bool isActive;
    public bool reset = true;
    public bool hooked;
    RaycastHit2D hit;
    public bool boolHelp;
    
    private Vector3 direction;
    
    public IEnumerator CurrentPath;
    

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        
        defaultHook = hookNode.transform.localPosition;
        defaultShoulder = hookNode.transform.localPosition;

        
        if(CurrentPath != null) { StopCoroutine(CurrentPath); }
        hookNode.GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If the hook is activated (always true as of now)
        if (isActive)
        {
            RenderLine();
            //Check if the hook is hanging 
                //If it is not and the right mouse is clicked throw hook
                
                //If it is render the arrow
                //Check if the w key is pressed
                    //If yes add force and recalculate the node positions
                //Else check if the right mouse is clicked again
                    //If yes, pull it in.
            
            if (reset)
            {
                
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Debug.Log(direction);
                    //hit = Physics2D.Raycast(transform.position, direction, MaxRange, whatIsGround);
                    //if (hit.collider != null) { CurrentPath = ThrowHit(); }
                    //else { CurrentPath = Throw(); }
                    CurrentPath = Throw(direction);
                    StartCoroutine(CurrentPath);
                }
            } else if (boolHelp)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1)) { StartCoroutine(Reel()); }
                if (Input.GetKeyUp(KeyCode.W)) { RecalculateNodes(); }
            }
        }
    }
    private void FixedUpdate()
    {
        if (isActive)
        {
            if (boolHelp)
            {
                
                if (Input.GetKey(KeyCode.W)) { Push(); }
            }
        }
    }

    public void FullReset() //Resets the positions to defaults
    {
        foreach (GameObject node in Nodes)
        {
            Destroy(node);
        }
        Nodes.Clear();
        reset = true;
        hooked = true;
        boolHelp = false;
        hookNode.GetComponent<Collider2D>().enabled = false;
        StopCoroutine(CurrentPath);
    }
    public void Push()
    {
        
        if(Nodes.Count > 0)
        {
            foreach (GameObject node in Nodes)
            {
                Destroy(node);
            }
            Nodes.Clear();
        }
        
        pushing = (hookNode.transform.position - player.transform.position);
        Vector2 mouseDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position);
        pushing = pushing.normalized;
        mouseDirection = mouseDirection.normalized;
        float angle = Vector2.Angle(mouseDirection, pushing);
        if(angle < fullForceAngle) { pushing = pushing + mouseDirection; }
        else if(angle < angleAllowance) { 
            mouseDirection = mouseDirection * ((angle - fullForceAngle) / (angleAllowance - fullForceAngle));
            pushing = pushing + mouseDirection; 
        }
        player.GetComponent<Rigidbody2D>().AddForce(pushing * thrownForce);
    }

    IEnumerator Throw(Vector3 target)
    {
        hooked = false;
        hookNode.GetComponent<Collider2D>().enabled = true;
        direction = (target - gameObject.transform.position).normalized;
        reset = false;
        hooked = false;
        while((gameObject.transform.localPosition-defaultHook).magnitude <= maxRange)
        {
            gameObject.transform.position = (Vector2)gameObject.transform.position + (Vector2)(direction * throwSpeed * Time.deltaTime);
            yield return null;
        }
        if((gameObject.transform.localPosition - defaultHook).magnitude > maxRange) { StartCoroutine(Reel()); }
        yield return null;
        //while it isn't at target, move towards target
        //If it gets too far away, end and start reeling it in.
        
    }

    void RenderLine()
    {
        if (Nodes.Count > 0)
        {
            lr.positionCount = Nodes.Count;

            for (int i = 0; i < Nodes.Count; i++)
            {

                lr.SetPosition(i, Nodes[i].transform.position);

            }

            //lr.SetPosition(5, player.transform.position);
        } else
        {
            lr.positionCount = 2;
            lr.SetPosition(0, shoulderNode.transform.position);
            lr.SetPosition(1, hookNode.transform.position);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Contact!");
        if(!hooked)
        {
            Debug.Log("Hit");
            boolHelp = true;
            StopCoroutine(CurrentPath);
            gameObject.transform.SetParent(collision.collider.gameObject.transform);
            RecalculateNodes();
            hooked = true;
        }
        //Check if the hook is being thrown
        //If yes then parent the hook to the hit object and set it to static and recalculate nodes
        //If enemy then start reeling it back in
        //If no, then do nothing

    }

    IEnumerator Reel()
    {
        hooked = true;
        boolHelp = false;
        Vector2 currVel = player.GetComponent<Rigidbody2D>().velocity;
        gameObject.transform.SetParent(player.transform);
        player.GetComponent<Rigidbody2D>().velocity = currVel;
        while (gameObject.transform.localPosition != defaultHook){
            gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, defaultHook, throwSpeed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Returned");
        FullReset();
        yield return null;
        //Ensure hooks parent is Player
        //Move hook towards shoulder point, then hook point
        
    }

    private void RecalculateNodes() {
        foreach(GameObject node in Nodes)
        {
            Destroy(node);
        }
        Nodes.Clear();
        //Start node is a static Rigidbody2D that has its points start at a natural spot
        GameObject startNode = (GameObject)Instantiate(baseNode, hookNode.transform.position, Quaternion.identity, hookNode.transform);
        Vector3 hookVec = (hookNode.transform.position - shoulderNode.transform.position);
        float hookMag = hookVec.magnitude;
        Vector3 nodeDistance =  hookVec / nodeCount;
        //New Node 1 is the first node which all others rotate around
        GameObject newNode = (GameObject)Instantiate(node, startNode.transform.position - nodeDistance, Quaternion.identity, hookNode.transform);
        newNode.GetComponent<HingeJoint2D>().connectedBody = startNode.GetComponent<Rigidbody2D>();
        //newNode.GetComponent<HingeJoint2D>().connectedAnchor = -(Vector2)(hookVec / (6.66666666f));
        Nodes.Add(startNode);
        Nodes.Add(newNode);
        GameObject lastNode = newNode;
        while (Nodes.Count <= nodeCount)
        {
            //Then instatniate a new node a distance away
            newNode = (GameObject)Instantiate(node, lastNode.transform.position - nodeDistance, Quaternion.identity, hookNode.transform);
            newNode.GetComponent<HingeJoint2D>().connectedBody = lastNode.GetComponent<Rigidbody2D>();
            //newNode.GetComponent<HingeJoint2D>().connectedAnchor = -(Vector2)(hookVec/(6.66666666f));
            Nodes.Add(newNode);
            lastNode = newNode;
        }
        player.GetComponent<HingeJoint2D>().enabled = true;
        player.GetComponent<HingeJoint2D>().connectedBody = lastNode.GetComponent<Rigidbody2D>();

        //lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
        //lastNode.GetComponent<HingeJoint2D>().connectedAnchor = -((startNode.transform.position - player.transform.position).normalized * nodeDistance);
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
