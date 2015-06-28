using UnityEngine;
using System.Collections;

public class BaseAI : MonoBehaviour 
{
	public int AtkDamage 
	{
		get;
		set;
	}

	public int Health
	{
		get;
		set;
	}

	public float MovementSpeed
	{
		get;
		set;
	}

	public float MinDist2Player 
	{
		get;
		set;
	}

	protected GameEventManager m_GameEventManager;
	
	public GameObject m_DeathParticlesPrefab;

	void Awake()
	{
		Health = 1;

		m_GameEventManager = FindObjectOfType<GameEventManager>();
	}
}
