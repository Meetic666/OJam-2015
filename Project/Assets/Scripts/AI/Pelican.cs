using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pelican : BaseAI 
{
	public float m_SinWaveDist;
	public float m_SeperationMul;

	public GameObject m_Player;

	float m_OriginalYpos;

	//Temp variables for quick changes
	public float m_MovementSpeed;

	List<GameObject> m_Obstacles = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
		AtkDamage = 5;

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
			m_OriginalYpos += (m_Player.transform.position.y - m_OriginalYpos) * Time.deltaTime;
			newPos.y += (m_OriginalYpos - newPos.y) * Time.deltaTime;
		}

		transform.position = Vector3.MoveTowards (transform.position, newPos, MovementSpeed);
	}

	//Seperation from obstacles
	Vector3 CalcSeperation()
	{
		Vector3 AveragePosition = Vector3.zero;
		float averageHeight = 0.0f;

		foreach(GameObject obstacle in m_Obstacles)
		{
			AveragePosition += obstacle.transform.position;
			BoxCollider obstacleCollider = obstacle.GetComponent<BoxCollider>();
			if(obstacleCollider != null)
			{
				averageHeight += obstacle.GetComponent<BoxCollider>().bounds.size.y;
			}
		}

		AveragePosition /= m_Obstacles.Count;
		averageHeight /= m_Obstacles.Count;

		AveragePosition.y += averageHeight;

		return AveragePosition;
	}

	void Attack(GameObject player)
	{
		m_GameEventManager.ReceiveEvent(GameEvent.e_PlayerHit, gameObject);

		Death ();
	}

	void Death()
	{
		if(m_DeathParticlesPrefab != null)
		{
			Instantiate (m_DeathParticlesPrefab, transform.position, Quaternion.identity);
		}

		gameObject.SetActive (false);
	}

	public void Damage(int dmg)
	{
		Health -= dmg;

		if(Health <= 0)
		{
			Death();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag != "Wall" && other.tag != "Player")
		{
			m_Obstacles.Add(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag != "Wall" && other.tag != "Player")
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
