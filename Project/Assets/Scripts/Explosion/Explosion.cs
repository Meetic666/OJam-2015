using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour 
{
	public float m_ExplosionRadius;
	public float m_ExplosionForce;
    public int m_Damage = 5;
    public float m_CameraShake = 1f;

    CameraController m_Camera;

	// Use this for initialization
	void Start () 
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius);

		foreach(Collider otherCollider in colliders)
		{
            BaseHealth<int> health = otherCollider.gameObject.GetComponent<BaseHealth<int>>();
            if (health != null)
            {
                health.Damage(m_Damage);
            }

			ExplodingElement explodingElement = otherCollider.GetComponent<ExplodingElement>() ;

			if(explodingElement != null)
			{
				explodingElement.Explode(transform.position, m_ExplosionRadius, m_ExplosionForce);
			}
		}

        m_Camera = Camera.main.GetComponent<CameraController>();
        if (m_Camera != null)
        {
            m_Camera.Shake(m_CameraShake);
        }
	}
}
