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
    RaycastHit2D hit;
    
    private Vector2 direction;
    
    IEnumerator CurrentPath;
    

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        
        defaultHook = hookNode.transform.localPosition;
        defaultShoulder = shoulderNode.transform.localPosition;

        Nodes.Add(shoulderNode);
        Nodes.Add(hookNode);
    }

    // Update is called once per frame
    void Update()
    {
        //If the hook is activated (always true as of now)
        if (isActive)
        {
            //Check if the hook is hanging 
                //If it is not and the right mouse is clicked throw hook
                
                //If it is render the arrow
                //Check if the w key is pressed
                    //If yes add force and recalculate the node positions
                //Else check if the right mouse is clicked again
                    //If yes, pull it in.
            
            if (CurrentPath == null)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
                    //hit = Physics2D.Raycast(transform.position, direction, MaxRange, whatIsGround);
                    //if (hit.collider != null) { CurrentPath = ThrowHit(); }
                    //else { CurrentPath = Throw(); }
                    StartCoroutine(CurrentPath);
                }
            }
        }
    }
    
    public void FullReset() //Resets the positions to defaults
    {

    }

    IEnumerator Throw(Vector3 target)
    {
        //while it isn't at target, move towards target
        //If it gets too far away, end and start reeling it in.
        yield return null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if the hook is being thrown
        //If yes then parent the hook to the hit object and set it to static and recalculate nodes
        //If enemy then start reeling it back in
        //If no, then do nothing

    }

    IEnumerator Reel()
    {
        //Ensure hooks parent is Player
        //Move hook towards shoulder point, then hook point
        yield return null;
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
