using UnityEngine;
using System.Collections;


public class CapMove : MonoBehaviour {

	Transform currentTarget;
	GameObject camOrb;
	Transform cameraOrbital;

	public bool showGUI = true;
	public bool topDown = false;
	bool oldTD = false;
	bool rotateR = false;
	bool rotateL = false;
	bool isTargeted = false;

	public int playerSpeed = 10;
	public int playerShotDis = 50;
	public int orbitCamSpeed = 10;

	public string moveMode = "vel";
	
	RaycastHit hit;

	void Start()
	{
		camOrb = GameObject.Find("CameraOrbital");
	}


	int rotCount = 0;

	void Update()
	{
		cameraOrbital = camOrb.transform;

		if(rotateR == true)										//Consolidate into function
		{ 
			cameraOrbital.eulerAngles += new Vector3(0,3,0);
			rotCount++;
			if (rotCount >= 30)
			{
				rotCount = 0;
				rotateR = false;
			}
		}
		if(rotateL == true)
		{
			cameraOrbital.eulerAngles -= new Vector3(0,3,0);
			rotCount++;
			if (rotCount >= 30)
			{
				rotCount = 0;
				rotateL = false;
			}
		}

		if (topDown != oldTD)
		{
			if (topDown == true)
			{
				cameraOrbital.eulerAngles += new Vector3(55,0,0);
			}
			else if(topDown == false)
			{
				cameraOrbital.eulerAngles -= new Vector3(55,0,0);
			}
		}
		if (isTargeted == true)
		{
			Debug.DrawRay(transform.position, currentTarget.transform.position - transform.position, Color.blue);
		}
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.transform.gameObject.tag == "Enemy")//Cast ray from sky to click point
			{
				currentTarget = hit.transform;
				isTargeted = true;
				if (Physics.Raycast(transform.position, currentTarget.transform.position - transform.position, out hit, playerShotDis) && hit.transform.gameObject.tag == "Enemy")//Shoot ray from player to target
				{
					TestAIScript a = hit.collider.gameObject.GetComponent<TestAIScript>();
					a.health -= 10;
					isTargeted = false;
					if (a.health <= 0)
					{
						Destroy(hit.collider.gameObject);
					}
				}
			}
		}
		oldTD = topDown;
	}

	void FixedUpdate () 
	{
		/*Start input code*/
		if (moveMode == "tran")
		{
			if (Input.GetKey(KeyCode.W))
			{
				transform.Translate(new Vector3(0,0,playerSpeed * Time.deltaTime));
			}
			else if (Input.GetKey(KeyCode.S))
			{
				transform.Translate(new Vector3(0,0,-playerSpeed * Time.deltaTime));
			}
			if (Input.GetKey (KeyCode.A))
			{
				transform.Translate(new Vector3(-playerSpeed * Time.deltaTime,0,0));
			}
			else if(Input.GetKey (KeyCode.D))
			{
				transform.Translate(new Vector3(playerSpeed * Time.deltaTime,0,0));
			}
		}
		if (moveMode == "vel")
		{
			if (Input.GetKey(KeyCode.W))
			{
				rigidbody.velocity = new Vector3(0, 0, playerSpeed);
			}
			if (Input.GetKey(KeyCode.S))
			{
				rigidbody.velocity = new Vector3(0, 0, -playerSpeed);
			}
			if (Input.GetKey(KeyCode.A))
			{
				rigidbody.velocity = new Vector3(-playerSpeed, 0, 0);
			}
			if (Input.GetKey(KeyCode.D))
			{
				rigidbody.velocity = new Vector3(playerSpeed, 0, 0);
			}
		}
		/*End input code*/
	}

	void OnGUI()
	{
		if (showGUI == true)
		{
			if (GUI.Button (new Rect (100,40,80,20), "--->")) 
			{
				rotateL = true;
			}
			if (GUI.Button (new Rect (20,40,80,20), "<---")) 
			{
				rotateR = true;
			}
			topDown = GUI.Toggle(new Rect(450, 10, 100, 30), topDown, "TopDown");
		}
	}
}
