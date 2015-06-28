using UnityEngine;
using System.Collections;

public class SideCollidersController : MonoBehaviour
{
    Transform m_PlayerTrans;
    public GameObject m_ExplosionPrefab;

	public float m_TimeBeforeExplosion = 5.0f;
	float m_Timer;

    // Use this for initialization
    void Start()
    {
        m_PlayerTrans = (Transform)GameObject.FindWithTag("Player").transform;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Sides"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Sides"), false);
    }

    // Update is called once per frame
    void Update()
    {
		if(m_PlayerTrans != null)
		{
        	transform.position = new Vector3(0f, 0f, m_PlayerTrans.position.z);
		}

		if(m_Timer > 0.0f)
		{
			m_Timer -= Time.deltaTime;
		}
    }

    void OnCollisionEnter(Collision collision)
    {
		if(m_Timer <= 0.0f)
		{
	        Instantiate(m_ExplosionPrefab, collision.contacts[0].point, Quaternion.identity);

			m_Timer = m_TimeBeforeExplosion;
		}
    }
}
