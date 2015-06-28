using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdSpawner : MonoBehaviour 
{
	public bool m_ClearForSpawning = false;
	public int m_BirdsPerWave;
	public GameObject m_BirdPrefab;

	public float m_WaveDelay;
	float m_WaveDelayTimer;

	public float m_PerBirdDelay;
	float m_PerBirdDelayTimer;

	GameObject m_Player;
	List<GameObject> m_Birds = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
		m_Player = FindObjectOfType<PlayerMovement> ().gameObject;
		m_WaveDelayTimer = m_WaveDelay;
		m_PerBirdDelayTimer = m_PerBirdDelay;

		for(int i = 0; i < m_BirdsPerWave; i++)
		{
			GameObject bird = (GameObject)Instantiate(m_BirdPrefab, transform.position, transform.rotation);
			bird.SetActive(false);
			m_Birds.Add(bird);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_ClearForSpawning)
		{
			if(m_WaveDelayTimer <= 0)
			{
				if(m_PerBirdDelayTimer <= 0)
				{
					foreach(GameObject bird in m_Birds)
					{
						if(bird.activeSelf == false)
						{
							bird.transform.position = transform.position;
							bird.SetActive(true);
							if(bird.GetComponent<Pelican>() == null)
							{
								bird.GetComponent<Mini_UFO>().Revive();
							}
							break;
						}
					}

					m_PerBirdDelayTimer = m_PerBirdDelay;
				}
				else
				{
					m_PerBirdDelayTimer -= Time.deltaTime;
				}
				m_WaveDelayTimer = m_WaveDelay;
			}
			else
			{
				m_WaveDelayTimer -= Time.deltaTime;
			}
		}

		if(m_Player != null)
		{
			transform.position = m_Player.transform.position - (transform.forward * 300);
		}
	}
}
