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
		e_SpecialOne
	}
	BehaviourStates m_CurrentState = BehaviourStates.e_Idle;

	public float m_DelayBetweenStates;
	float m_DelayBetweenStatesTimer;

	public GameObject m_Projectile;
	List<GameObject> m_Debris = new List<GameObject> ();

	public float m_DelayBetweenShots;
	float m_DelayBetweenShotsTimer = 0;

	public float m_LiftSpeed;
	public float m_MinDistance2Boss;
	public int m_ShotsPerBurst;
	public GameObject m_Player;

	float m_PatienceMultiplier;
	int m_CurrentShotsFired = 0;

	//Temp variables 
	public int m_Health;

	// Use this for initialization
	void Start () 
	{
		Health = m_Health;
		m_DelayBetweenStatesTimer = m_DelayBetweenStates;
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(m_CurrentState)
		{
			case BehaviourStates.e_Attack:
			{
				if(m_DelayBetweenShotsTimer <= 0)
				{
					ShootTurrets();
					m_CurrentShotsFired++;
					m_DelayBetweenShotsTimer = m_DelayBetweenShots;
				}
				else
				{
					m_DelayBetweenShotsTimer = m_DelayBetweenShots;
				}

				if(m_CurrentShotsFired == m_ShotsPerBurst)
				{
					int rand = Random.Range(0, 2);

					if(rand > 0)
					{
						ChangeStateTo(BehaviourStates.e_SpecialOne);
					}
					else
					{
						ChangeStateTo(BehaviourStates.e_Attack);
					}
					m_CurrentShotsFired = 0;
				}
				break;
			}
		
		case BehaviourStates.e_SpecialOne:
		{
			RaycastHit hit;

			if(Physics.SphereCastAll(transform.position, 2 /* Temp number */, -transform.up, transform.position.y))
			{
				if(hit.collider.tag == "Movable")
				{
					//Tell the object to stop moving

					m_Debris.Add(hit.collider.gameObject);
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
				}
				else
				{
					//Lift objects in m_Debris
					debris.transform.position += m_LiftSpeed;
				}
			}

			break;
		}

		default:
			break;
		}
	}

	void ChangeStateTo(BehaviourStates newState)
	{
		//Add effects here for when the Boss changes states
		switch(newState)
		{
			case BehaviourStates.e_Idle:
			{
				break;
			}
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

		m_CurrentState = newState;
	}

	void ShootTurrets()
	{

	}
}
