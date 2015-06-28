using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour, BaseHealth<int> {

    Rigidbody m_Body;
    const float GRAVITY = 10f;
    const float SPEED = 40f;
    const float CONTROL_ACCELERATION = 2.5f;

	GameEventManager m_GameEventManager;

	// Use this for initialization
	void Start ()
    {
        m_Body = (Rigidbody)gameObject.GetComponent<Rigidbody>();

		m_GameEventManager = FindObjectOfType<GameEventManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Turn
        Vector3 turn = Vector3.zero;

        Vector3 autoTurn = Quaternion.FromToRotation(transform.forward, Vector3.forward).eulerAngles;
        if (autoTurn.x > 180f)
        {
            autoTurn.x = -(360f - autoTurn.x);
        }
        if (autoTurn.y > 180f)
        {
            autoTurn.y = -(360f - autoTurn.y);
        }
        if (autoTurn.z > 180f)
        {
            autoTurn.z = -(360f - autoTurn.z);
        }

        //Turn
        if (Vector3.Dot(transform.forward, Vector3.forward) > 0.5f)
        {
            if (Input.GetKey(KeyCode.A))
            {
                turn.y -= CONTROL_ACCELERATION;
            }
            if (Input.GetKey(KeyCode.D))
            {
                turn.y += CONTROL_ACCELERATION;
            }
            if (Input.GetKey(KeyCode.W))
            {
                turn.x -= CONTROL_ACCELERATION;
            }
            if (Input.GetKey(KeyCode.S))
            {
                turn.x += CONTROL_ACCELERATION;
            }
        }
        turn += autoTurn * 0.04f;

        /*if (transform.rotation.z > 0.5f)
        {
            turn.z -= 0.5f;
        }
        if (transform.rotation.z < -0.5f)
        {
            turn.z += 0.5f;
        }*/

        m_Body.angularVelocity = new Vector3(m_Body.angularVelocity.x, m_Body.angularVelocity.y, -transform.rotation.z) + turn * Time.deltaTime;

        Vector3 force = SPEED * transform.forward;
        m_Body.AddForce(force);
	}

	public void Damage(int dmg)
	{
		m_GameEventManager.ReceiveEvent(GameEvent.e_PlayerHit, null, dmg);
	}

    void OnCollisionEnter(Collision collision)
    {
        //Just here for testing, may be changed to the player asploding
        CameraController cam = Camera.main.GetComponent<CameraController>();
        cam.Shake(1f);
    }
}

