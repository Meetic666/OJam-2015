using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss_UFO : BaseAI 
{
	enum BehaviourStates
	{
		e_Idle = 0,
		e_Attack,
		e_Valnerable,
		e_SpecialOne,
		e_Death
	}
	BehaviourStates[] m_StateQueue = new BehaviourStates[2];

	public float m_DelayBetweenStates;
	float m_DelayBetweenStatesTimer;

	public GameObject m_Projectile;
	List<GameObject> m_Debris = new List<GameObject> ();

	public float m_DelayBetweenShots;
	float m_DelayBetweenShotsTimer = 0;

	public float m_DurationOfCurrentState;
	float m_DurationOfCurrentStateTimer;

	public float m_DelayUntilBossFight;
	public Vector3 m_LiftSpeed;
	public float m_MinDistance2Boss;
	public int m_ShotsPerBurst;
	public GameObject m_Player;
	public GameObject[] m_Turrets = new GameObject[2];

	float m_PatienceMultiplier = 1;
	float m_GameTime = 0.0f;
	int m_CurrentShotsFired = 0;

	//Temp variables 
	public int m_Health;
	public float m_MinDist2Player;

	// Use this for initialization
	void Start () 
	{
		Health = m_Health;
		MinDist2Player = m_MinDist2Player;

		m_DelayBetweenStatesTimer = m_DelayBetweenStates;

		m_StateQueue [0] = BehaviourStates.e_Idle;
		m_StateQueue [1] = BehaviourStates.e_Attack;

		m_DurationOfCurrentStateTimer = m_DurationOfCurrentState;
	}

	// Update is called once per frame
	void Update () 
	{
		m_GameTime += Time.deltaTime;

		RunState (m_StateQueue [0]);
		m_DurationOfCurrentStateTimer -= Time.deltaTime;

		if(m_DurationOfCurrentStateTimer <= 0)
		{
			ChangeState();
			m_DurationOfCurrentStateTimer = m_DurationOfCurrentState;
		}

		if(m_DelayBetweenStatesTimer <= 0)
		{
			if(m_StateQueue[1] != BehaviourStates.e_Valnerable)
			{
				RunState(m_StateQueue[1]);
			}
		}
		else
		{
			if(m_StateQueue[0] != BehaviourStates.e_Idle)
			{
				m_DelayBetweenStatesTimer -= Time.deltaTime;
			}
		}

		//Debug.Log (m_StateQueue [0].ToString() + " " + m_StateQueue [1].ToString());
	}

	void RunState(BehaviourStates state)
	{
		switch(state)
		{
		case BehaviourStates.e_Valnerable:
		{

			break;
		}

		case BehaviourStates.e_Idle:
		{
			if(m_GameTime >= m_DelayUntilBossFight)
			{
				QueueNewState(BehaviourStates.e_Attack);
			}
			else
			{
				Vector3 newPos = Vector3.zero;
				newPos.z = transform.position.z + (MinDist2Player - transform.position.z) * Time.deltaTime;
			}
			
			break;
		}
			
		case BehaviourStates.e_Attack:
		{
			if(m_DelayBetweenShotsTimer <= 0)
			{
				if(m_CurrentShotsFired >= m_ShotsPerBurst)
				{
					m_DelayBetweenShotsTimer = m_DelayBetweenShots * m_PatienceMultiplier;
					ChangeState();
				}
				else
				{
					ShootTurrets();
					m_CurrentShotsFired++;
				}
			}
			else
			{	
				m_DelayBetweenShotsTimer -= Time.deltaTime;
			}
			break;
		}
			
		case BehaviourStates.e_SpecialOne:
		{
			RaycastHit[] hit;
			
			hit = Physics.SphereCastAll(transform.position, 2 /* Temp number */, -transform.up, transform.position.y);
			
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
					debrisPhysics.AddForce(m_Player.transform.position - debris.transform.position, ForceMode.Impulse);
					
					m_Debris.Remove(debris);
				}
				else
				{
					//Lift objects in m_Debris
					debris.transform.position += m_LiftSpeed;
				}
			}

			if(m_PatienceMultiplier >= 2)
			{
				m_StateQueue[1] = BehaviourStates.e_Attack;
			}

			break;
		}

		case BehaviourStates.e_Death:
		{
			transform.position -= transform.up * 0.07f;

			break;
		}
			
		default:
			break;
		}
	}

	void QueueNewState(BehaviourStates newState)
	{
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
				break;
			}
		default:
			break;
		}

		m_StateQueue [1] = newState;
	}

	void ChangeState()
	{
		if(m_StateQueue[1] == BehaviourStates.e_Idle)
		{
			int rand = Random.Range (0, 6);
			if(rand > 2)
			{
				QueueNewState(BehaviourStates.e_Attack);
			}
			else if(rand > 0)
			{
				QueueNewState(BehaviourStates.e_SpecialOne);
			}
			else
			{
				QueueNewState(BehaviourStates.e_Valnerable);
			}
		}
		
		//Add effects here for when the Boss changes states
		switch(m_StateQueue[1])
		{
			case BehaviourStates.e_Attack:
			{
				break;
			}
			case BehaviourStates.e_SpecialOne:
			{
				break;
			}
			case BehaviourStates.e_Valnerable:
			{
				//Reveal Core to player
				break;
			}
		default:
			break;
		}

		m_DelayBetweenStatesTimer = m_DelayBetweenStates * m_PatienceMultiplier;
		
		m_StateQueue [0] = m_StateQueue [1];
		m_StateQueue [1] = BehaviourStates.e_Idle;
	}

	void ShootTurrets()
	{
		//Shoot projectiles at player
		GameObject projectile = (GameObject)Instantiate (m_Projectile);

		projectile.transform.position = m_Turrets[m_CurrentShotsFired % 2].transform.position;
		projectile.transform.eulerAngles = m_Player.transform.position - m_Turrets [m_CurrentShotsFired % 2].transform.position;

		m_CurrentShotsFired++;
	}

	void Death()
	{
		QueueNewState (BehaviourStates.e_Death);
		m_DelayBetweenStatesTimer = 0;

		//Notify Event Manager
	}

	void CalculateDamage(int dmg)
	{
		if(m_StateQueue[0] == BehaviourStates.e_Valnerable)
		{
			Health -= dmg;
			if(Health <= 0)
			{
				Death ();
			}
		}
		else
		{
			m_PatienceMultiplier += 0.1f;
		}
	}
}
