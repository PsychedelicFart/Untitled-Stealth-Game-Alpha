using UnityEngine;
using System.Collections;

public class TestAIScript : MonoBehaviour {
	RaycastHit hit;

	TextMesh healthBar;
	NavMeshAgent ai;

	public LayerMask a;
	public LayerMask b;

	public int health = 100;

	public float x = 0, y = 0, z = 0;
	float yy;
	float sinY;
	float cosY;
	public float aiSight = 30;
	public float deacTimer;
	public float deactivationTime;

	Vector3 aiVec;

	public Transform player;

	public bool showRays = false;
	bool moveToPlayer = false;
	bool playerSpotted = false;
	bool[] rayHitPlayer =  new bool[8];
	
	void Start()
	{
		healthBar = gameObject.GetComponentInChildren<TextMesh>();
		ai = gameObject.GetComponent<NavMeshAgent>();
	}

	void Update () 
	{
		healthBar.text = health.ToString();
		RayCheck();
		Debug.DrawRay(transform.position, aiVec);
		if (deacTimer < deactivationTime && playerSpotted == true)
		{
			moveToPlayer = true;
		}
		if (RayActivated () == true)
		{
			deacTimer = 0;
			playerSpotted = true;
		}
		else
		{
			deacTimer += Time.deltaTime;
			if (deacTimer > deactivationTime)
			{
				moveToPlayer = false;
			}
		}
		if (moveToPlayer == true)
		{
			ai.SetDestination (player.position);
		}
	}

	bool RayActivated()
	{
		int hitCount = 0;//Debug feature
		for (int i = 0; i < 8; i++)
		{
			if(rayHitPlayer[i] == true)
			{
				hitCount++;
			}
		}
		if (hitCount >= 1)
		{
			return true;
		}
		return false;
	}
					
	void RayCheck()
	{
		int j = 0;
		for(float i = -4; i < 4; i+=1)
		{
			y = (gameObject.transform.eulerAngles.y+(i*10))* Mathf.Deg2Rad;
			sinY = Mathf.Sin(y);
			cosY = Mathf.Cos(y);
			aiVec = new Vector3(sinY,0,cosY);
			if (showRays == true)
			{
				Debug.DrawRay(transform.position, aiVec * aiSight);
			}
			if (Physics.Raycast(transform.position, aiVec, out hit ,aiSight) && hit.transform.gameObject.tag == "Player")
			{
				rayHitPlayer[j] = true;
			}
			else
			{
				rayHitPlayer[j] = false;
			}
			j++;
		}
	}
}
