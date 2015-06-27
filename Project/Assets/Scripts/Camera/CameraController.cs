using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    Transform m_PlayerTrans;
    Rigidbody m_Body;
    float m_ChaseDist = 0f;
    const float LEPR_SPEED_PRE_DELTA = 3f;
    const float LEPR_SIDE_SPEED_PRE_DELTA = 3f;
    const float LOOK_LERP_PRE_DELTA = 5f;

    const float LERP_DISTANCE = 0.1f;
    const float LOOK_FORWARD_AMOUNT = 10f;
    Vector3 lastLookatAt;

	// Use this for initialization
	void Start ()
    {
        m_PlayerTrans = (Transform)GameObject.FindWithTag("Player").transform;
        m_Body = m_PlayerTrans.gameObject.GetComponent<Rigidbody>();
        m_ChaseDist = transform.position.z - m_PlayerTrans.position.z;
        lastLookatAt = m_PlayerTrans.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 finalPos = transform.position;
        finalPos.z = Mathf.Lerp(transform.position.z, m_PlayerTrans.position.z + m_ChaseDist, LEPR_SPEED_PRE_DELTA * Time.deltaTime);

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 playPos = new Vector2(m_PlayerTrans.position.x, m_PlayerTrans.position.y);

        if (Vector2.Distance(pos, playPos) > LERP_DISTANCE)
        {
            pos = Vector2.Lerp(pos, playPos, Mathf.Min(LEPR_SIDE_SPEED_PRE_DELTA * Time.deltaTime, 1f));
            finalPos.x = pos.x;
            finalPos.y = pos.y;
        }
        transform.position = finalPos;
        Debug.Log(transform.position);

        lastLookatAt = Vector3.Lerp(lastLookatAt, m_PlayerTrans.position + m_Body.velocity.normalized * LOOK_FORWARD_AMOUNT, Mathf.Min(Time.deltaTime * LOOK_LERP_PRE_DELTA, 1f));
        transform.LookAt(lastLookatAt);
	}
}
