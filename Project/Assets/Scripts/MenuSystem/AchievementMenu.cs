using UnityEngine;
using System.Collections;

public class AchievementMenu : MonoBehaviour 
{
	public SpriteRenderer[] m_SpriteRenderers;

	public GameEventManager m_GameEventManager;
	
	// Update is called once per frame
	void OnEnable()
	{
		for(int i = 0; i < m_SpriteRenderers.Length; i++)
		{
			m_SpriteRenderers[i].enabled = m_GameEventManager.IsAchievementUnlocked((AchievementType) i);
		}
	}
}
