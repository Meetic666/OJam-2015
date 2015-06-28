using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public GameObject m_Projectile;
    public float FiringSpeed = 1f;
    public float ShakeOnFire = 1f;
    public float RandomOffset = 0f;

	public AudioSource m_WeaponSound;

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

    public void Fire(Vector3 startPos)
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
                    SpawnProjectile(startPos);
                }
            }
            else
            {
				SpawnProjectile(startPos);
			}
		}

		if(m_WeaponSound != null && !m_WeaponSound.isPlaying)
		{
			m_WeaponSound.Play ();
		}
    }

	public void EndFire()
	{
		if(m_WeaponSound != null)
		{
			m_WeaponSound.Stop ();
		}
	}

    void SpawnProjectile(Vector3 startPos)
    {

        Vector3 offset = Vector3.zero;
        if (RandomOffset > 0f)
        {
			offset += transform.up * Random.Range(-RandomOffset, RandomOffset);
			offset += transform.right * Random.Range(-RandomOffset, RandomOffset);
        }
		Vector3 spawnPos = transform.position + startPos * 6f + offset;
        GameObject newProjectile = (GameObject) GameObject.Instantiate(m_Projectile, spawnPos, Quaternion.identity);
		newProjectile.tag = tag;
		newProjectile.layer = gameObject.layer;
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
