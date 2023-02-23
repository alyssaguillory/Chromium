using UnityEngine;
using System.Collections;

public class ThrowHook : MonoBehaviour
{


	public GameObject hook;


	public bool currActive;

	public GameObject curHook;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{



		if (Input.GetMouseButtonDown(1))
		{


			if (curHook == null)
			{
				Vector2 destiny = Camera.main.ScreenToWorldPoint(Input.mousePosition);


				curHook = (GameObject)Instantiate(hook, transform.position, Quaternion.identity);

				curHook.GetComponent<RopeScript>().destiny = destiny;
				//curHook.transform.SetParent(transform);


				RopeScript.ropeActive = true;
			}
			else
			{

				//delete rope

				//Destroy(curHook);
				curHook.GetComponent<RopeScript>().returning = true;
				RopeScript.ropeActive = false;
				//curHook.GetComponent<RopeScript>().Return();



			}
		}



	}
}