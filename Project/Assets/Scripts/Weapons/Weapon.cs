using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public GameObject m_Projectile;
    public float FiringSpeed = 1f;
    public float ShakeOnFire = 1f;
    public float RandomOffset = 0f;

    CameraController m_Camera;
    float m_FiringTimer = -1f;

    void Start()
    {
        m_Camera = Camera.main.GetComponent<CameraController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        update();
        if (!CanFire())
        {
            m_FiringTimer -= Time.deltaTime;
        }
	}

    protected virtual void update()
    {
    }

    public void Fire()
    {
        if (CanFire())
        {
            m_FiringTimer = FiringSpeed;
            m_Camera.Shake(ShakeOnFire);
            int numberFired = (int)(Time.deltaTime / FiringSpeed);
            if (numberFired++ > 1)
            {
                for (int i = 0; i < numberFired; i++)
                {
                    SpawnProjectile();
                }
            }
            else
            {
                SpawnProjectile();
            }
        }
    }

    void SpawnProjectile()
    {
        Vector3 offset = Vector3.zero;
        if (RandomOffset > 0f)
        {
            offset += transform.up * Random.Range(-RandomOffset, RandomOffset);
            offset += transform.right * Random.Range(-RandomOffset, RandomOffset);
        }
        Vector3 spawnPos = transform.position + transform.forward * 6f + offset;
        GameObject.Instantiate(m_Projectile, spawnPos, Quaternion.FromToRotation(Vector3.forward, (spawnPos - transform.position).normalized));
    }

    public bool CanFire()
    {
        if (m_FiringTimer <= 0f)
        {
            return true;
        }
        return false;
    }
}
