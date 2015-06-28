using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
    public float m_Speed;
    Rigidbody m_Body;

	public GameObject m_HitParticles;

    void Start()
    {
        m_Body = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Body.velocity = transform.forward * m_Speed;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
		if(collisionInfo.gameObject.tag != tag)
		{
	        BaseHealth<int> health = collisionInfo.gameObject.GetComponent<BaseHealth<int>>();
	        if (health != null)
	        {
	            health.Damage(1);
	        }

			if(m_HitParticles != null)
			{
				Instantiate (m_HitParticles, transform.position, Quaternion.identity);
			}

	        Destroy(gameObject);
		}
    }
}
