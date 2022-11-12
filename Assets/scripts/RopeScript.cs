using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RopeScript : MonoBehaviour
{

	public Vector2 destiny;

	public float speed = 1;
	public float MaxRange = 10;

	public float climbDistance = 0.4f;
	public float climbSpeed = 0.4f;

	public float distance = 2;

	public GameObject nodePrefab;

	public GameObject player;

	public GameObject lastNode;

	public LineRenderer lr;
	private bool Attached;

	int vertexCount = 2;
	public List<GameObject> Nodes = new List<GameObject>();


	public bool done = false;
	static public bool ropeActive = true;

	public Vector2 velocityKeeper;
	private Vector2 direction;
	public LayerMask whatIsGround;
	RaycastHit2D hit;
	public bool returning = false;
	Vector2 missPoint;

	// Use this for initialization
	void Start()
	{


		lr = GetComponent<LineRenderer>();

		player = GameObject.FindGameObjectWithTag("Player");

		lastNode = transform.gameObject;


		Nodes.Add(transform.gameObject);
		direction = destiny - (Vector2)transform.position;
		hit = Physics2D.Raycast(transform.position, direction, MaxRange, whatIsGround);
		missPoint = (Vector2)transform.position + Vector2.ClampMagnitude(direction, MaxRange);
	}

	// Update is called once per frame
	void Update()
	{
		if (hit.collider != null && ropeActive == true)
		{

			if ((Vector2)transform.position != hit.point)
			{
				transform.position = Vector2.MoveTowards(transform.position, hit.point, speed);
				if (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance)
				{


					CreateNode();

				}


			}
			else if (done == false)
			{
				GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
				done = true;

				CreateNode();

				while (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance * 2)
				{
					CreateNode();
				}


				lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
			}

			if (Input.GetKey(KeyCode.W) && (Vector2)transform.position == hit.point)
			{
				MoveUp();
			}

		}
		else
		{
			Return();
		}

		RenderLine();
	}


	void RenderLine()
	{

		lr.SetVertexCount(vertexCount);

		int i;
		for (i = 0; i < Nodes.Count; i++)
		{

			lr.SetPosition(i, Nodes[i].transform.position);

		}

		lr.SetPosition(i, player.transform.position);

	}


	void CreateNode()
	{

		Vector2 pos2Create = player.transform.position - lastNode.transform.position;
		pos2Create.Normalize();
		pos2Create *= distance;
		pos2Create += (Vector2)lastNode.transform.position;

		GameObject go = (GameObject)Instantiate(nodePrefab, pos2Create, Quaternion.identity, transform);

		lastNode.GetComponent<HingeJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();

		lastNode = go;


		Nodes.Add(lastNode);

		vertexCount++;

	}

	void MoveUp()
	{
		if (Nodes.Count > 4)
		{
			if ((Nodes[Nodes.Count - 1].transform.position - player.transform.position).magnitude < climbDistance)
			{
				Nodes[Nodes.Count - 2].GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
				Destroy(Nodes[Nodes.Count - 1]);
				Nodes.RemoveAt(Nodes.Count - 1);
				lastNode = Nodes[Nodes.Count - 1];
				vertexCount--;

				MoveTowards();
			}
			else { MoveTowards(); }

		}
		else
		{
			ropeActive = false;
		}
	}

	void MoveTowards()
	{
		Nodes[Nodes.Count - 1].GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;

		Vector2 normalToPlayer = (Nodes[Nodes.Count - 1].GetComponent<HingeJoint2D>().connectedAnchor).normalized;			
		Nodes[Nodes.Count - 1].GetComponent<HingeJoint2D>().connectedAnchor = Nodes[Nodes.Count - 1].GetComponent<HingeJoint2D>().connectedAnchor - (normalToPlayer * climbSpeed);
		//player.transform.position = Nodes[Nodes.Count - 1].transform.position;
	}

	public void Return()
	{
		if (returning == false)
		{
			transform.position = Vector2.MoveTowards(transform.position, missPoint, speed);
			if ((Vector2)transform.position == missPoint) { returning = true; }
		}
		else
		{
			RemoveNodes();
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * 2.0f);
			if (transform.position == player.transform.position)
			{
				ropeActive = false;
				Destroy(transform.gameObject);
			}
		}
	}

	void RemoveNodes()
	{
		while (Nodes.Count > 1)
		{
			Destroy(Nodes[Nodes.Count - 1]);
			Nodes.RemoveAt(Nodes.Count - 1);
			vertexCount--;
			Nodes[Nodes.Count - 1].GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
		}
		//Destroy(Nodes[1]);
		//lastNode = Nodes[0];
		transform.gameObject.GetComponent<HingeJoint2D>().enabled = false;
	}
}