using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pelican : BaseAI 
{
	public float m_SinWaveDist;
	public float m_SeperationMul;

	float m_OriginalYpos;

	//Temp variables for quick changes
	public float m_MovementSpeed;

	List<GameObject> m_Obstacles = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
		MovementSpeed = m_MovementSpeed;
		MinDist2Player = 0;

		m_OriginalYpos = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Movement
		Vector3 newPos = transform.position;

		newPos.z += (MinDist2Player - newPos.z) * Time.deltaTime;
		newPos.x += Mathf.Sin (Time.time * MovementSpeed) * m_SinWaveDist;

		if(m_Obstacles.Count > 0)
		{
			Vector3 seperationDist = CalcSeperation();
			newPos.y -= (seperationDist.y * m_SeperationMul);
		}
		else
		{
			newPos.y += (m_OriginalYpos - newPos.y) * Time.deltaTime;
		}

		transform.position = Vector3.MoveTowards (transform.position, newPos, MovementSpeed);
	}

	//Seperation from obstacles
	Vector3 CalcSeperation()
	{
		Vector3 AveragePosition = Vector3.zero;

		foreach(GameObject obstacle in m_Obstacles)
		{
			AveragePosition += obstacle.transform.position;
		}

		AveragePosition /= m_Obstacles.Count;

		return AveragePosition;
	}

	void Attack(GameObject player)
	{
		//Deal damage to player

		Death ();
	}

	void Death()
	{
		//Add effects here for Death

		gameObject.SetActive (false);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag != "Wall")
		{
			m_Obstacles.Add(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag != "Wall")
		{
			m_Obstacles.Remove(other.gameObject);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Player")
		{
			Attack(other.gameObject);
		}
	}
}
