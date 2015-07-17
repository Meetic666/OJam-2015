using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour, BaseHealth<int> {

    Rigidbody m_Body;
    const float GRAVITY = 10f;
    const float SPEED = 4000f;
    const float CONTROL_ACCELERATION = 1.5f;

	GameEventManager m_GameEventManager;

	public AudioSource m_JetSound;

	float m_CollisionMultiplier = 1.0f;

	float m_CollisionTimer;

	ScoreManager m_PlayerScore;

	Vector3 m_TargetPosition;

	public Vector3 m_MaxPosition;
	public float m_MaxRotation;

	Vector3 m_PreviousMousePosition;

	// Use this for initialization
	void Start ()
    {
		m_PlayerScore = FindObjectOfType<ScoreManager> ();

        m_Body = (Rigidbody)gameObject.GetComponent<Rigidbody>();

		m_GameEventManager = FindObjectOfType<GameEventManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(transform.eulerAngles.y > 70 && transform.eulerAngles.y < 290)
		{
			m_GameEventManager.ReceiveEvent(GameEvent.e_PlayerHit, null, 100);
		}


		if(Time.timeScale != 0.0f)
		{
			if(!m_JetSound.isPlaying)
			{
				m_JetSound.Play ();
			}
		}
		else
		{
			m_JetSound.Pause();
		}

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

		if(!Input.anyKey)
		{
			turn += autoTurn * 0.2f * m_CollisionMultiplier;
		}
		else if(Input.GetKey (KeyCode.Mouse0) || Input.GetKey (KeyCode.Mouse1))
		{
			turn += autoTurn * 0.2f * m_CollisionMultiplier;
		}

        /*if (transform.rotation.z > 0.5f)
        {
            turn.z -= 0.5f;
        }
        if (transform.rotation.z < -0.5f)
        {
            turn.z += 0.5f;
        }*/

        m_Body.angularVelocity = new Vector3(m_Body.angularVelocity.x, m_Body.angularVelocity.y, -transform.rotation.z) + turn * Time.deltaTime;

		if(Time.timeScale != 0.0f)
		{
	        Vector3 force = SPEED * transform.forward * Time.deltaTime;
	        m_Body.AddForce(force);
		}

		if(m_CollisionTimer > 0.0f)
		{
			m_CollisionTimer -= Time.deltaTime;

			if(m_CollisionTimer >= 0.0f)
			{
				m_CollisionMultiplier = 1.0f;
			}
		}
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

		if(collision.gameObject.tag == "Sides")
		{
			m_CollisionMultiplier = 2.0f;
			m_CollisionTimer = 2.0f;
		}
    }
}

