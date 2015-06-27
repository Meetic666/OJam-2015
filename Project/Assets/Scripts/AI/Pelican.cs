using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pelican : BaseAI 
{
	public float m_SinWaveDist;
	public float m_SeperationMul;

	//Temp variables for quick changes
	public float m_MovementSpeed;
	public float m_MinDist2Player;

	List<GameObject> m_Obstacles = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
		MovementSpeed = m_MovementSpeed;
		MinDist2Player = m_MinDist2Player;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Movement
		Vector3 newPos = transform.position;

		newPos.z = newPos.z + (MinDist2Player - newPos.z) * Time.deltaTime;
		newPos.x = newPos.x + Mathf.Sin (Time.time * MovementSpeed) * m_SinWaveDist;

		if(m_Obstacles.Count > 0)
		{
			Vector3 seperationDist = CalcSeperation();
			newPos.y = transform.position.y + ((seperationDist.y * m_SeperationMul) * 
			                                   ((m_Obstacles [0].transform.position.y - transform.position.y) / 
			 									(m_Obstacles [0].transform.position.y - transform.position.y)));
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
}
