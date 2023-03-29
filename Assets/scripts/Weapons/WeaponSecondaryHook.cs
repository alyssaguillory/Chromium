using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSecondaryHook : WeaponBase
{
    public float maxRange = 60, throwSpeed = 3;
    public float moveVelocity = 300, angleAllowance = 45, fullForceAngle=30;
    
    public Vector3 target;

    [SerializeField] GameObject player;
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
    
    private Vector3 direction;
    
    public IEnumerator CurrentPath;
    

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        
        defaultHook = hookNode.transform.localPosition;
        defaultShoulder = shoulderNode.transform.localPosition;

        
        if(CurrentPath != null) { StopCoroutine(CurrentPath); }
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
            } else if (hooked)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1)) { StartCoroutine(Reel()); }
            }
        }
    }
    
    public void FullReset() //Resets the positions to defaults
    {
        reset = true;
        hooked = false;
        StopCoroutine(CurrentPath);
        foreach (GameObject node in Nodes)
        {
            Destroy(node);
        }
    }

    IEnumerator Throw(Vector3 target)
    {
        
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
        if (Nodes.Count > 2)
        {
            lr.SetVertexCount(Nodes.Count + 1);

            for (int i = 0; i < Nodes.Count; i++)
            {

                lr.SetPosition(i, Nodes[i].transform.position);

            }

            lr.SetPosition(5, player.transform.position);
        } else
        {
            lr.SetVertexCount(2);
            lr.SetPosition(0, player.transform.position);
            lr.SetPosition(1, transform.position);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Contact!");
        if(!hooked)
        {
            Debug.Log("Hit");
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
        gameObject.transform.SetParent(player.transform);
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
        int i = 0;
        GameObject lastNode = (GameObject)Instantiate(node, transform.position, Quaternion.identity, transform);
        lastNode.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        lastNode.GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
        lastNode.GetComponent<HingeJoint2D>().connectedAnchor = lastNode.transform.position;
        while (Nodes.Count < 4)
        {
            i++;
            GameObject go = (GameObject)Instantiate(node, transform.position - (transform.position-player.transform.position)*(i/5), Quaternion.identity, transform);

            go.GetComponent<HingeJoint2D>().connectedBody = lastNode.GetComponent<Rigidbody2D>();
            go.GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;

            //lastNode = go;

            
            Nodes.Add(go);
            lastNode = go;
        }
        player.GetComponent<HingeJoint2D>().connectedBody = lastNode.GetComponent<Rigidbody2D>();
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