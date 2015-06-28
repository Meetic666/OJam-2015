using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mini_UFO : BaseAI , BaseHealth<int>
{
	public float m_SinWaveDist;
	public float m_SeperationMul;
	public GameObject m_Projectile;
	GameObject m_Player;
	
	float m_OriginalYpos;

	public float m_AtkCoolDown;
	float m_AtkCoolDownTimer;
	
	//Temp variables for quick changes
	public float m_MovementSpeed;
	public float m_MinDist2Player;
	public int m_Health;
	
	List<GameObject> m_Obstacles = new List<GameObject>();
	
	// Use this for initialization
	void Start () 
	{
		MovementSpeed = m_MovementSpeed;
		MinDist2Player = m_MinDist2Player;
		Health = m_Health;
		
		m_OriginalYpos = transform.position.y;
		m_AtkCoolDownTimer = m_AtkCoolDown;

		m_Player = FindObjectOfType<PlayerMovement> ().gameObject;

		ScoreIncrease = 2500;
	}
	
	// Update is called once per frame
	void Update () 
	{

		//Movement
		Vector3 newPos = transform.position;

		if(m_Player != null)
		{
			newPos.z += ((m_Player.transform.position.z + MinDist2Player) - newPos.z) * Time.deltaTime * MovementSpeed;
		}

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

		//Combat
		if(m_AtkCoolDownTimer <= 0)
		{
			Attack();
			m_AtkCoolDownTimer = m_AtkCoolDown;
		}
		else
		{
			m_AtkCoolDownTimer -= Time.deltaTime;
		}
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
	
	void Attack()
	{
		if(m_Player != null)
		{
		//Launch projectile at player
		GameObject projectile = (GameObject)Instantiate (m_Projectile);
		projectile.transform.position = transform.position + transform.forward;
		projectile.transform.forward = m_Player.transform.position - transform.position;
		}
	}
	
	void Death()
	{
		if(m_DeathParticlesPrefab != null)
		{
			Instantiate (m_DeathParticlesPrefab, transform.position, Quaternion.identity);
		}
		
		m_GameEventManager.ReceiveEvent(GameEvent.e_EnemyKilled, gameObject, ScoreIncrease);

		if(m_DeathSound != null)
		{
			Instantiate (m_DeathSound, transform.position, Quaternion.identity);
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

	public void Revive()
	{
		Health = m_Health;
	}

	void OnTriggerEnter(Collider other)
	{
		/*if(other.tag != "Wall" && other.tag != "Enemy" && other.tag != "Player" && other.tag != "Projectile")
		{
			//m_Obstacles.Add(other.gameObject);
		}*/
	}
	
	void OnTriggerExit(Collider other)
	{
		/*if(other.tag != "Wall" && other.tag != "Enemy" && other.tag != "Player" && other.tag != "Projectile")
		{
			//m_Obstacles.Remove(other.gameObject);
		}*/
	}

}
