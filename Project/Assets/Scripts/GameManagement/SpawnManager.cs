using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour 
{
	public GameObject m_PelicanPrefab;
	public GameObject m_MiniUFOPrefab;

	//Temporarily public
	public int m_PelicansPerWave;
	public int m_MiniUFOsPerWave;

	public float m_DelayBetweenSpawns;
	float m_DelayBetweenSpawnsTimer;
	public float m_SpawnMultiplier;

	public float m_DelayBetweenWaves;
	float m_DelayBetweenWavesTimer;

	//
	int m_SpawnedOnCurrentWave = 0;
	List<GameObject> m_Enemies = new List<GameObject>();
	List<GameObject> m_Obstacles = new List<GameObject>();

	// Update is called once per frame
	void Update () 
	{
		if(m_DelayBetweenWavesTimer <= 0)
		{
			if(m_DelayBetweenSpawnsTimer <= 0)
			{
				int rand = (int)Random.Range(0, 6);
				if((rand % 2) == 0)
				{

				}
			}
		}
	}

	public void StartNextStage()
	{
		//Spawn additional wave and change percentage values
	}
}
