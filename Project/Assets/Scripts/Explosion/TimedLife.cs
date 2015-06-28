using UnityEngine;
using System.Collections;

public class TimedLife : MonoBehaviour {

    float m_Timer = 3f;

	// Update is called once per frame
	void Update ()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer < 0f)
        {
            GameObject.Destroy(gameObject);
        }
	}
}
