using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
	GameEventManager m_GameEventManager;

	public GameObject m_SplashMenu;
	public GameObject m_MainMenu;
	public GameObject m_PauseMenu;

	// Use this for initialization
	void Start () 
	{
		m_GameEventManager = FindObjectOfType<GameEventManager>();
	}

	void Update()
	{
		m_PauseMenu.SetActive(m_GameEventManager.GamePaused);
	}

	public void DoAction(ActionType action)
	{
		switch(action)
		{
		case ActionType.e_Achievements:
			break;

		case ActionType.e_AnyKey:
			m_SplashMenu.SetActive (false);
			m_MainMenu.SetActive (true);
			break;

		case ActionType.e_Leaderboard:
			break;

		case ActionType.e_Quit:
			Application.Quit();
			break;

		case ActionType.e_Start:
			m_GameEventManager.ReceiveEvent(GameEvent.e_GameLaunched, null);
			m_MainMenu.SetActive (false);
			break;

		case ActionType.e_Resume:
			m_GameEventManager.ReceiveEvent(GameEvent.e_GamePaused, null);
			m_PauseMenu.SetActive(false);
			break;
		}
	}
}
