using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public GameObject m_Projectile;
    public float FiringSpeed = 1f;
    public float ShakeOnFire = 1f;

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
            GameObject.Instantiate(m_Projectile, transform.position + transform.forward * 4f, Quaternion.FromToRotation(Vector3.forward, transform.forward));
        }
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
