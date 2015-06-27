using UnityEngine;
using System.Collections;

public class SideCollidersController : MonoBehaviour
{
    Transform m_PlayerTrans;

    // Use this for initialization
    void Start()
    {
        m_PlayerTrans = (Transform)GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0f, 0f, m_PlayerTrans.position.z);
    }
}
