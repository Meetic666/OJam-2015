using UnityEngine;
using System.Collections;

public class LeaderboardMenu : MonoBehaviour 
{
	public GameEventManager m_GameEventManager;
	public MenuManager m_MenuManager;
	
	string m_ParsedLeaderboard = "";
	
	public Rect m_BackButtonPosition;
	
	void OnEnable()
	{
		m_GameEventManager.GetLeaderboard("Global Leaderboard");
	}
	
	public void ParseLeaderboard()
	{
		int players = m_GameEventManager.m_LeaderboardData["social_leaderboard"].Count;
		m_ParsedLeaderboard = "";
		
		for (int i = 0; i < players; i++)
		{
			m_ParsedLeaderboard += (i + 1) + "\t";
			m_ParsedLeaderboard += m_GameEventManager.m_LeaderboardData["social_leaderboard"][i]["name"].ToString() + "\t";
			m_ParsedLeaderboard += m_GameEventManager.m_LeaderboardData["social_leaderboard"][i]["score"] + "\n";
		}
	}
	
	void OnGUI()
	{
		m_BackButtonPosition = Camera.main.pixelRect;
		m_BackButtonPosition.width *= 0.2f;
		m_BackButtonPosition.height *= 0.1f;
		m_BackButtonPosition.x = (Camera.main.pixelRect.width - m_BackButtonPosition.width) * 0.5f;
		m_BackButtonPosition.y = Camera.main.pixelRect.height - m_BackButtonPosition.height;

		Rect m_TextRect = Camera.main.pixelRect;
		m_TextRect.width = m_BackButtonPosition.width;
		m_TextRect.x = m_BackButtonPosition.x;
		m_TextRect.height = m_TextRect.height - m_BackButtonPosition.height;

		GUI.TextField(m_TextRect, m_ParsedLeaderboard);
		
		if(GUI.Button(m_BackButtonPosition, "Back"))
		{
			m_MenuManager.DoAction(ActionType.e_BackToMenu);
		}
	}
}
