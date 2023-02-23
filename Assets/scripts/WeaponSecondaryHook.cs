using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSecondaryHook : MonoBehaviour
{
    public GameObject player;
    public float maxDist;
    public float spacing;
    public GameObject node;
    public float climbSpeed;
    public Vector3 target;
    public bool done;
    public float throwSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //Change to coroutine
    void Throw()
    {
        if((target-transform.position).magnitude == 0) { done = true; }
        if((transform.position-player.transform.position).magnitude >= maxDist && !done)
        {
            Retract();
            return;
        }
        //Problems here probably
        else if (!done) {  transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * throwSpeed); }
        else
        {
            //setup nodes
        }
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
}
