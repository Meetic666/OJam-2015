using UnityEngine;
using System.Collections;

public class Car : ExplodingElement 
{
	public int m_ScoreDecrease = 1000;

	public GameObject m_DeathSound;

	virtual public void Explode(Vector3 positionOfExplosion, float radiusOfExplosion, float forceOfExplosion)
	{
		base.Explode(positionOfExplosion, radiusOfExplosion, forceOfExplosion);

		if(m_DeathSound != null)
		{
			Instantiate (m_DeathSound, transform.position, Quaternion.identity);
		}

		m_GameEventManager.ReceiveEvent(GameEvent.e_CivilianKilled, gameObject, m_ScoreDecrease);
	}
}
