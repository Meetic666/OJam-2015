using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
	GameEventManager m_GameEventManager;

	public GameObject m_SplashMenu;
	public GameObject m_MainMenu;
	public GameObject m_PauseMenu;
	public GameObject m_GameMenu;
	public GameObject m_AchievementMenu;
	public GameObject m_LeaderboardMenu;

	// Use this for initialization
	void Start () 
	{
		m_GameEventManager = FindObjectOfType<GameEventManager>();
	}

	void Update()
	{
		Cursor.visible = (m_GameEventManager.CurrentState == GameState.e_MainMenu || m_GameEventManager.GamePaused || m_GameEventManager.CurrentState == GameState.e_End);
	}

	public void DoAction(ActionType action)
	{
		switch(action)
		{
		case ActionType.e_Achievements:
			m_AchievementMenu.SetActive(true);
			m_MainMenu.SetActive (false);
			m_PauseMenu.SetActive(false);
			break;

		case ActionType.e_AnyKey:
			m_SplashMenu.SetActive (false);
			m_MainMenu.SetActive (true);
			break;

		case ActionType.e_Leaderboard:
			m_LeaderboardMenu.SetActive(true);
			m_MainMenu.SetActive(false);
			m_PauseMenu.SetActive(false);
			break;

		case ActionType.e_Quit:
			m_GameEventManager.ReceiveEvent(GameEvent.e_GameExited, null, 0);
			break;

		case ActionType.e_Start:
			m_GameEventManager.ReceiveEvent(GameEvent.e_GameLaunched, null, 0);
			m_GameMenu.SetActive (true);
			m_MainMenu.SetActive (false);
			break;

		case ActionType.e_Resume:
			m_GameEventManager.ReceiveEvent(GameEvent.e_GamePaused, null, 0);
			m_PauseMenu.SetActive(false);
			break;

		case ActionType.e_BackToMenu:
			m_LeaderboardMenu.SetActive(false);
			m_AchievementMenu.SetActive(false);
			m_MainMenu.SetActive(!m_GameEventManager.GamePaused);
			m_PauseMenu.SetActive(m_GameEventManager.GamePaused);
			break;
		}
	}

	void OnGUI()
	{
		if(Event.current.type == EventType.KeyDown)
		{
			if(Input.GetKeyDown (KeyCode.Escape))
			{
				m_GameEventManager.ReceiveEvent(GameEvent.e_GamePaused, null , 0);

				m_PauseMenu.SetActive(m_GameEventManager.GamePaused);
			}

			if(m_SplashMenu.activeSelf)
			{
				DoAction(ActionType.e_AnyKey);
			}
		}
	}
}
