using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour 
{
	public float m_ExplosionRadius;
	public float m_ExplosionForce;

	// Use this for initialization
	void Start () 
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius);

		foreach(Collider otherCollider in colliders)
		{
			ExplodingElement explodingElement = otherCollider.GetComponent<ExplodingElement>() ;

			if(explodingElement != null)
			{
				explodingElement.Explode(transform.position, m_ExplosionRadius, m_ExplosionForce);
			}
		}
	}
}
