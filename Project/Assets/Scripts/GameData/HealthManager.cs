using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour 
{
	public GameObject m_HealthBar;
	public float m_MaxHealth = 100.0f;

	public int m_RegenerationRate = 1;

	public GameObject m_DeathParticlesPrefab;

	public float m_TimeBeforeRegeneration = 10.0f;

	float m_Timer;

	float m_CurrentHealth;

	float m_MaxScale;

	GameEventManager m_GameEventManager;

	// Use this for initialization
	void Start () 
	{
		m_CurrentHealth = m_MaxHealth;

		m_MaxScale = m_HealthBar.transform.localScale.x;

		m_GameEventManager = FindObjectOfType<GameEventManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_Timer -= Time.deltaTime;

		if(m_Timer <= 0.0f)
		{
			m_CurrentHealth += m_RegenerationRate * Time.deltaTime;

			if(m_CurrentHealth > m_MaxHealth)
			{
				m_CurrentHealth = m_MaxHealth;
			}
		}

		float scalePercentage = m_CurrentHealth / m_MaxHealth;
		scalePercentage = Mathf.Clamp01(scalePercentage);

		Vector3 newScale = m_HealthBar.transform.localScale;
		newScale.x = scalePercentage * m_MaxScale;
		m_HealthBar.transform.localScale = newScale;
	}

	public void Hit(float damage)
	{
		m_Timer = m_TimeBeforeRegeneration;

		m_CurrentHealth -= damage;

		if(m_CurrentHealth <= 0.0f)
		{
			Die ();
		}
	}

	void Die()
	{
		m_GameEventManager.ReceiveEvent(GameEvent.e_PlayerKilled, null);

		if(m_DeathParticlesPrefab != null)
		{
			Instantiate(m_DeathParticlesPrefab, transform.position, Quaternion.identity);
		}

		// TODO: Destroy player
	}
}
