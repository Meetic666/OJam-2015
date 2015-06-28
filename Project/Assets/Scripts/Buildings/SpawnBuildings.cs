using UnityEngine;
using System.Collections.Generic;

public class SpawnBuildings : MonoBehaviour {

    List<GameObject> m_Buildings;
    List<GameObject> m_BuildingPrefabs;
    float m_LastBuildingPosRight = 0f;
    float m_LastBuildingPosLeft = 0f;
    float m_Timer = TIMER;
    const float TIMER = 0.35f;

    List<GameObject> m_Roads;
    List<GameObject> m_RoadPrefabs;
    float m_LastRoadPos = 0f;

	float m_RoadOffset = 296.4f;

	// Use this for initialization
	void Start ()
    {
        int index = 0;
        m_Buildings = new List<GameObject>();
        m_BuildingPrefabs = new List<GameObject>();

        GameObject building = (GameObject)Resources.Load("Prefabs/Buildings/Building" + index);
        while (building != null)
        {
            m_BuildingPrefabs.Add(building);
            index++;
            building = (GameObject)Resources.Load("Prefabs/Buildings/Building" + index);
        }

        if (m_BuildingPrefabs.Count == 0)
        {
            Destroy(this);
        }
        else
        {
            //Spawn buildings to start
            for (int i = 0; i < 30; i++ )
            {
                SpawnBuilding();
            }
        }

        index = 0;
        m_Roads = new List<GameObject>();
        m_RoadPrefabs = new List<GameObject>();

        GameObject road = (GameObject)Resources.Load("Prefabs/Roads/Road" + index);
        while (road != null)
        {
            m_RoadPrefabs.Add(road);
            index++;
            road = (GameObject)Resources.Load("Prefabs/Roads/Road" + index);
        }

        if (m_RoadPrefabs.Count == 0)
        {
            Destroy(this);
        }
        else
        {
            SpawnRoad();
            SpawnRoad();
            SpawnRoad();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < m_Buildings.Count; i++)
        {
            if (m_Buildings[i].transform.position.z < transform.position.z - 50f - m_Buildings[i].transform.localScale.x * 0.5f)
            {
                GameObject.Destroy(m_Buildings[i]);
                m_Buildings.RemoveAt(i);
            }
        }

        for (int i = 0; i < m_Roads.Count; i++)
        {
            if (m_Roads[i].transform.position.z < transform.position.z - m_RoadOffset * 0.5f)
            {
                GameObject.Destroy(m_Roads[i]);
                m_Roads.RemoveAt(i);
                SpawnRoad();
            }
        }

        m_Timer -= Time.deltaTime;
        if (m_Timer < 0f)
        {
            m_Timer = TIMER;
            SpawnBuilding();
        }
    }

    void SpawnBuilding()
    {
        //New Building
        GameObject building = (GameObject)GameObject.Instantiate(m_BuildingPrefabs[Random.Range(0, m_BuildingPrefabs.Count)]);

        //Size
        Vector3 buildingSize = Vector3.one * Random.Range(0.25f, 4f);
        building.transform.localScale = Vector3.Scale(buildingSize, building.transform.localScale);

        //Position
        int side = Random.Range(0, 2);
        if (side == 0)
        {
            m_LastBuildingPosLeft += building.transform.localScale.x * 0.5f + Random.Range(3f, 10f);
            float dist = 30f + building.transform.localScale.z * 0.25f;
            building.transform.position = new Vector3(side * dist * 2 - dist, building.transform.localScale.y * 0.5f - 15f, m_LastBuildingPosLeft);
            m_LastBuildingPosLeft += building.transform.localScale.x * 0.5f;
        }
        else
        {
            m_LastBuildingPosRight += building.transform.localScale.x * 0.5f + Random.Range(2f, 10f);
            float dist = 30f + building.transform.localScale.z * 0.25f;
            building.transform.position = new Vector3(side * dist * 2 - dist, building.transform.localScale.y * 0.5f - 17f, m_LastBuildingPosRight);
            m_LastBuildingPosRight += building.transform.localScale.x * 0.5f;
        }

        //Rotation
        Vector3 lookAt = building.transform.position;
        lookAt.x = 0f;
        building.transform.LookAt(lookAt);

        //Add
        m_Buildings.Add(building);
    }

    void SpawnRoad()
    {
        //New Road
		GameObject road = (GameObject)GameObject.Instantiate(m_RoadPrefabs[Random.Range(0, m_RoadPrefabs.Count)]);
		road.transform.position = new Vector3 (0f, -16f, m_LastRoadPos);
        m_LastRoadPos += m_RoadOffset;
        m_Roads.Add(road);
    }
}
