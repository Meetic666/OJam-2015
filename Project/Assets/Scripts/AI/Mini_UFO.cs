﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mini_UFO : BaseAI 
{
	public float m_SinWaveDist;
	public float m_SeperationMul;
	public GameObject m_Projectile;
	public GameObject m_Player;
	
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
		//Launch projectile at player
		GameObject projectile = (GameObject)Instantiate (m_Projectile);
		projectile.transform.position = transform.position;
		projectile.transform.eulerAngles = m_Player.transform.position - transform.position;

		//Set speed of projectile
	}
	
	void Death()
	{
		//Add effects here for Death
		
		gameObject.SetActive (false);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag != "Wall" || other.tag != "Enemy" || other.tag != "Player")
		{
			m_Obstacles.Add(other.gameObject);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.tag != "Wall" || other.tag != "Enemy" || other.tag != "Player")
		{
			m_Obstacles.Remove(other.gameObject);
		}
	}

}