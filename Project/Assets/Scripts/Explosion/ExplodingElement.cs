using UnityEngine;
using System.Collections;

public class ExplodingElement : MonoBehaviour
{
	public GameObject m_CollisionEffectsPrefab;

	Rigidbody m_Rigidbody;

	protected GameEventManager m_GameEventManager;

	// Use this for initialization
	void Start () 
	{
		m_Rigidbody = GetComponent<Rigidbody>();

		m_GameEventManager = GetComponent<GameEventManager>();
	}

	virtual public void Explode(Vector3 positionOfExplosion, float radiusOfExplosion, float forceOfExplosion)
	{
		if(m_Rigidbody != null)
		{
			m_Rigidbody.isKinematic = false;

			m_Rigidbody.AddExplosionForce(forceOfExplosion, positionOfExplosion, radiusOfExplosion);
		}
	}

	void OnCollisionEnter(Collision collisionInfo)
	{
		if(m_CollisionEffectsPrefab != null)
		{
			Instantiate (m_CollisionEffectsPrefab, collisionInfo.contacts[0].point, Quaternion.identity);
		}
	}
}
