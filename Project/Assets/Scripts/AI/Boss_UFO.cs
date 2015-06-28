using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss_UFO : BaseAI , BaseHealth<int>
{
	enum BehaviourStates
	{
		e_Idle = 0,
		e_Attack,
		e_Valnerable,
		e_SpecialOne,
		e_Death
	}
	BehaviourStates[] m_StateQueue = new BehaviourStates [2];

	public float m_DelayBetweenStates;
	float m_DelayBetweenStatesTimer;

	public GameObject m_Projectile;
	List<GameObject> m_Debris = new List<GameObject> ();

	public float m_DelayBetweenShots;
	float m_DelayBetweenShotsTimer = 0;

	bool m_MinionsCleared = true;

	public GameObject m_TractorBeam;
	public float m_ThrowingForce;
	public Vector3 m_LiftSpeed;
	public float m_MinDistance2Boss;
	public int m_ShotsPerBurst;
	GameObject m_Player;
	public GameObject[] m_Turrets = new GameObject[2];

	float m_PatienceMultiplier = 1;
	int m_CurrentShotsFired = 0;

	//Temp variables 
	public int m_Health;
	public float m_MinDist2Player;
	public float m_MovementSpeed;

	// Use this for initialization
	void Start () 
	{
		Health = m_Health;
		MinDist2Player = m_MinDist2Player;
		MovementSpeed = m_MovementSpeed;
		m_DelayBetweenStatesTimer = m_DelayBetweenStates;

		m_StateQueue [0] = BehaviourStates.e_Idle;
		m_StateQueue [1] = BehaviourStates.e_Attack;

		m_Player = FindObjectOfType<PlayerMovement> ().gameObject;

		ScoreIncrease = 1000000;
	}
	
	// Update is called once per frame
	void Update () 
	{
		RunState (m_StateQueue [0]);
		m_DelayBetweenStatesTimer -= Time.deltaTime;

		if(m_DelayBetweenStatesTimer <= 0)
		{
			if(m_StateQueue[1] != BehaviourStates.e_Valnerable && m_StateQueue[0] != BehaviourStates.e_Idle)
			{
				RunState(m_StateQueue[1]);
			}
		}

		if(m_StateQueue[0] != BehaviourStates.e_Idle && m_StateQueue[0] != BehaviourStates.e_Death)
		{
			Vector3 newPos = transform.position;

			newPos.z = m_Player.transform.position.z + MinDist2Player;

			transform.position = newPos;
		}
	}

	void RunState(BehaviourStates state)
	{
		switch(state)
		{
		case BehaviourStates.e_Idle:
		{
			if(MinDist2Player <= Vector3.Distance(m_Player.transform.position + 
			                                      ((transform.position - m_Player.transform.position).normalized * MinDist2Player), 
			                                      transform.position))
			{
				Vector3 newPos = transform.position;
					
				newPos += (m_Player.transform.position - transform.position).normalized * Time.deltaTime * MovementSpeed;
				newPos.y = transform.position.y;

				transform.position = newPos;
			}
			else
			{
				ChangeState();

				//m_GameEventManager.ReceiveEvent(GameEvent.e_EngagedBoss, null, 0);
			}

			break;
		}
		case BehaviourStates.e_Attack:
		{
			if(m_DelayBetweenShotsTimer <= 0)
			{
				ShootTurrets();
				m_DelayBetweenShotsTimer = m_DelayBetweenShots;
			}
			else
			{
				m_DelayBetweenShotsTimer -= Time.deltaTime;
			}

			if(m_CurrentShotsFired >= m_ShotsPerBurst)
			{
				ChangeState();
			}

			break;
		}
			
		case BehaviourStates.e_SpecialOne:
		{
			RaycastHit[] hit;

			hit = Physics.SphereCastAll(transform.position, 5 /* Temp number */, -transform.up, 10);
				
			foreach(RaycastHit obstacle in hit)
			{
				if(obstacle.collider.tag == "Movable")
				{
					//Tell the object to stop moving

					m_Debris.Add(obstacle.collider.gameObject);
				}
			}
			
			foreach(GameObject debris in m_Debris)
			{
				float distance2Boss = (transform.position - debris.transform.position).magnitude;
				
				if(distance2Boss <= m_MinDistance2Boss)
				{
					//Throw objects at player when they reach the max height
					Rigidbody debrisPhysics = debris.GetComponent<Rigidbody>();
					debrisPhysics.useGravity = true;
					Vector3 force = (m_Player.transform.position - debris.transform.position).normalized * m_ThrowingForce;
					debrisPhysics.AddForce(force, ForceMode.Impulse);

				}
				else
				{
					//Lift objects in m_Debris
					debris.transform.position += m_LiftSpeed;
				}
				m_Debris.Remove(debris);
				if(m_Debris.Count <= 0)
				{
					break;
				}
			}
			
			break;
		}
			
		default:
			break;
		}
	}

	void ChangeState()
	{

		if(m_StateQueue[1] == BehaviourStates.e_Idle)
		{
			int rand = (int)Random.Range(0, 6);
			if(rand > 3)
			{
				m_StateQueue[1] = BehaviourStates.e_Attack;
				m_DelayBetweenStatesTimer = m_DelayBetweenStates * m_PatienceMultiplier;
			}
			else if(rand > 1)
			{
				m_TractorBeam.SetActive(true);
				m_StateQueue[1] = BehaviourStates.e_SpecialOne;
				m_DelayBetweenStatesTimer = m_DelayBetweenStates * m_PatienceMultiplier;
			}
			else
			{
				m_StateQueue[1] = BehaviourStates.e_Valnerable;
				m_DelayBetweenStatesTimer = (m_DelayBetweenStates /2) * m_PatienceMultiplier;
			}
		}

		//Reset values of state that just ended
		switch(m_StateQueue[0])
		{
			case BehaviourStates.e_Attack:
			{
				m_CurrentShotsFired = 0;

				break;
			}
			case BehaviourStates.e_SpecialOne:
			{
				foreach(GameObject debris in m_Debris)
				{
					m_Debris.Remove(debris);
				}

				m_TractorBeam.SetActive(false);

				break;
			}
		default:
			break;
		}

		m_StateQueue[0] = m_StateQueue[1];
		m_StateQueue [1] = BehaviourStates.e_Idle;
	}

	void ShootTurrets()
	{
		GameObject projectile = (GameObject)Instantiate (m_Projectile);

		int rand = (int)Random.Range (0, 2);

		projectile.transform.position = m_Turrets [rand % 2].transform.position;
		projectile.transform.eulerAngles = (m_Turrets [rand % 2].transform.position - transform.position).normalized;

		m_CurrentShotsFired++;
	}

	public void Damage(int dmg)
	{
		Health -= dmg;
		
		if(Health <= 0)
		{
			m_StateQueue[1] = BehaviourStates.e_Death;
			
			m_GameEventManager.ReceiveEvent(GameEvent.e_EnemyKilled, gameObject, ScoreIncrease);

			ChangeState();
		}
	}
}
