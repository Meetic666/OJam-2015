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

	public int ScoreIncrease
	{
		get;
		set;
	}

	protected GameEventManager m_GameEventManager;
	
	public GameObject m_DeathParticlesPrefab;

	public GameObject m_DeathSound;

	void Awake()
	{
		Health = 1;

		m_GameEventManager = FindObjectOfType<GameEventManager>();
	}
}
