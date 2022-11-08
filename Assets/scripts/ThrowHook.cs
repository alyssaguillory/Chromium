using UnityEngine;
using System.Collections;

public class ThrowHook : MonoBehaviour
{


	public GameObject hook;


	public bool ropeActive;

	GameObject curHook;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{



		if (Input.GetMouseButtonDown(0))
		{


			if (ropeActive == false)
			{
				Vector2 destiny = Camera.main.ScreenToWorldPoint(Input.mousePosition);


				curHook = (GameObject)Instantiate(hook, transform.position, Quaternion.identity);

				curHook.GetComponent<RopeScript>().destiny = destiny;
				//curHook.transform.SetParent(transform);


				ropeActive = true;
			}
			else
			{

				//delete rope

				Destroy(curHook);


				ropeActive = false;

			}
		}


	}
}