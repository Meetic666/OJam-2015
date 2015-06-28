using UnityEngine;
using System.Collections;

public enum AchievementType
{
	e_StillTrying,
	e_FourthOfJuly,
	e_DrunkDriver,
	e_WillSmith,
	e_LastProtector,
	e_TrueFriend
}

public class AchievementsManager : MonoBehaviour 
{
	float m_TimeThatAchievementShown = 5.0f;
	float m_Timer;

	SpriteRenderer m_AchievementBanner;

	public Sprite[] m_AchievementSprites;

	// Use this for initialization
	void Start () 
	{
		m_AchievementBanner = GetComponentInChildren<SpriteRenderer>();
		m_AchievementBanner.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_Timer > 0.0f)
		{
			m_Timer -= Time.deltaTime;

			if(m_Timer <= 0.0f)
			{
				m_AchievementBanner.enabled = false;
			}
		}
	}

	public void ShowAchievement(AchievementType achievement)
	{
		m_AchievementBanner.sprite = m_AchievementSprites[(int) achievement];
		m_AchievementBanner.enabled = true;

		m_Timer = m_TimeThatAchievementShown;
	}
}
