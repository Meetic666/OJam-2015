using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour 
{
	public GameObject m_PelicanPrefab;
	public GameObject m_MiniUFOPrefab;
	public GameObject[] m_VehiclePrefabs = new GameObject[3];

	public GameObject m_Player;

	public GameObject m_EnemySpawnPoint;
	public GameObject m_VehicleSpawnPoint;
	public GameObject[] m_PedestrianSpawnPoints = new GameObject[2];

	//Temporarily public
	public int m_PelicansPerWave;
	public int m_MiniUFOsPerWave;

	public float m_PedestrianSpawnDelay;
	float m_PedestrianSpawnTimer;

	public float m_DelayBetweenSpawns;
	float m_DelayBetweenSpawnsTimer;
	public float m_SpawnMultiplier;

	public float m_DelayBetweenWaves;
	float m_DelayBetweenWavesTimer;

	//
	public int m_MaxSpawnsPerWave;
	int m_SpawnedOnCurrentWave = 0;
	int m_RandomNumb = 0;
	List<GameObject> m_Enemies = new List<GameObject>();
	List<GameObject> m_Obstacles = new List<GameObject>();

	// Update is called once per frame
	void Update () 
	{
		if(m_DelayBetweenWavesTimer <= 0)
		{
			m_RandomNumb = (int)Random.Range(0, 6);
			if(m_DelayBetweenSpawnsTimer <= 0)
			{
				if((m_RandomNumb % 2) == 0)
				{
					GameObject enemy = (GameObject)Instantiate(m_MiniUFOPrefab);
//					enemy.transform.position = m_EnemySpawnPoint + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
//					enemy.transform.eulerAngles = m_Player.transform.position - m_EnemySpawnPoint.transform.position;

					m_DelayBetweenSpawnsTimer = m_DelayBetweenSpawns * 1.5f;
				}
				else
				{
					GameObject enemy = (GameObject)Instantiate(m_PelicanPrefab);
//					enemy.transform.position = m_EnemySpawnPoint + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
//					enemy.transform.eulerAngles = m_Player.transform.position - m_EnemySpawnPoint.transform.position;
					
					m_DelayBetweenSpawnsTimer = m_DelayBetweenSpawns;
				}
			}
			else
			{
				m_DelayBetweenSpawnsTimer -= Time.deltaTime;
			}

			m_SpawnedOnCurrentWave++;

			if(m_SpawnedOnCurrentWave >= m_MaxSpawnsPerWave)
			{
				m_DelayBetweenWavesTimer = m_DelayBetweenWaves;
			}
		}
		else
		{
			m_DelayBetweenWavesTimer -= Time.deltaTime;
		}

		if(m_PedestrianSpawnTimer <= 0)
		{
			int rand = (int) Random.Range(0, 4);
			if(rand == 0)
			{

			}
		}
	}

	public void StartNextStage()
	{
		//Spawn additional wave and change percentage values
	}
}
