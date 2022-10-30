using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook1 : MonoBehaviour
{
    public GameObject ropeShooter;
    public LineRenderer lineRenderer;
    public float freq = 4f;
    public float dampening = 4f;
    public bool grappling = false;
    public LayerMask whatIsGround;

    private SpringJoint2D rope;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!grappling)
            {
                Fire();
            } else
            {
                Snip();
            }
        }
    }
    void LateUpdate()
    {
        if (grappling)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount=2;
            lineRenderer.SetPosition(0, ropeShooter.transform.position);
            lineRenderer.SetPosition(1, rope.connectedAnchor);
            //lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        } else
        {
            lineRenderer.enabled = false;
        }
    }
    void Fire()
    {
        Vector3 mousePositionPixel = Input.mousePosition;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePositionPixel);
        Vector3 position = ropeShooter.transform.position;
        Vector3 direction = mousePosition - position;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, Mathf.Infinity, whatIsGround);
        if(hit.collider != null)
        {
            SpringJoint2D newRope = ropeShooter.AddComponent<SpringJoint2D>();
            newRope.enableCollision = true;
            newRope.frequency = freq;
            newRope.connectedAnchor = hit.point;
            newRope.enabled = true;
            newRope.dampingRatio = dampening;
            newRope.autoConfigureDistance = false;
            grappling = true;

            GameObject.DestroyImmediate(rope);
            rope = newRope;
        }
    }
    void Snip()
    {
        GameObject.DestroyImmediate(rope);
        grappling = false;
    }
}
