using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour 
{
	public float m_Speed;
	public float m_RotationSpeed;

	public GameObject m_ExplosionPrefab;

	public GameObject Target
	{
		get;
		set;
	}

	void Start()
	{
		RaycastHit hit; 
		Physics.SphereCast (transform.position, 1, transform.forward, out hit, 1 << 10);

		if(hit.collider != null && hit.collider.tag != "Wall" && hit.collider.tag != "Missle" && hit.collider.tag != "Player")
		{
			Target = hit.collider.gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//UnityEditor.EditorApplication.isPaused = true;
		transform.position += transform.forward * m_Speed * Time.deltaTime;

		if(Target != null)
		{
			Vector3 targetDirection = Target.transform.position - transform.position;
			targetDirection.Normalize();

			float distanceToTarget = targetDirection.magnitude;

			if(Target != null)
			{
				//transform.forward = Vector3.Slerp (transform.forward, targetDirection, m_RotationSpeed * Time.deltaTime);
			}

		}
	}

	void OnCollisionEnter(Collision collisionInfo)
	{
		if(collisionInfo.gameObject.tag != tag && m_ExplosionPrefab != null)
		{
			Instantiate(m_ExplosionPrefab, collisionInfo.contacts[0].point, Quaternion.identity);

			Destroy(gameObject);
		}
	}
}
