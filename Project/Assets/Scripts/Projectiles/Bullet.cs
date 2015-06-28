using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float m_Speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * m_Speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {

        //if ()
        {
            Destroy(gameObject);
        }
    }
}
