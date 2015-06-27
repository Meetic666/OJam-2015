using UnityEngine;
using System.Collections;

public enum GameEvent
{
	e_GameLaunched,
	e_GamePaused,
	e_EnemyKilled,
	e_CivilianKilled
}

public class GameEventManager : MonoBehaviour 
{
	GameState m_CurrentState = GameState.e_MainMenu;

	bool m_GamePaused = false;

	public GameState CurrentState
	{
		get
		{
			return m_CurrentState;
		}
	}

	public bool GamePaused
	{
		get
		{
			return m_GamePaused;
		}
	}

	public void ReceiveEvent(GameEvent gameEvent, GameObject target)
	{
		switch(gameEvent)
		{
		case GameEvent.e_GameLaunched:
			OnGameLaunched();
			break;

		case GameEvent.e_GamePaused:
			OnGamePaused();
			break;

		case GameEvent.e_EnemyKilled:
			OnEnemyKilled(target);
			break;
		}
	}

	void OnGameLaunched()
	{
		if(m_CurrentState == GameState.e_MainMenu)
		{
			m_CurrentState = GameState.e_GamePart1;
		}
	}

	void OnGamePaused()
	{
		if(m_CurrentState == GameState.e_GamePart1 || m_CurrentState == GameState.e_GamePart2)
		{
			m_GamePaused = !m_GamePaused;

			Time.timeScale = m_GamePaused ? 0.0f : 1.0f;
		}
	}

	void OnEnemyKilled(GameObject target)
	{
		if(!m_GamePaused && (m_CurrentState == GameState.e_GamePart1 || m_CurrentState == GameState.e_GamePart2))
		{
			// TODO: increment score
		}
	}

	[ContextMenu("Move One State")]
	void MoveOneState()
	{
		m_CurrentState = (GameState)((int) m_CurrentState + 1);
	}

	[ContextMenu("Pause game")]
	void PauseGame()
	{
		ReceiveEvent(GameEvent.e_GamePaused, null);
	}
}
