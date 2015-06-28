using UnityEngine;
using System.Collections;

public enum GameEvent
{
	e_GameLaunched,
	e_GamePaused,
	e_EnemyKilled,
	e_CivilianKilled,
	e_PlayerHit,
	e_PlayerKilled
}

public class GameEventManager : MonoBehaviour 
{
	float m_TimeBeforeReset = 5.0f;
	float m_Timer;

	GameState m_CurrentState = GameState.e_MainMenu;

	bool m_GamePaused = false;

	public HealthManager m_PlayerHealth;
	public ScoreManager m_PlayerScore;

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

	void Update()
	{
		if(m_Timer > 0.0f)
		{
			m_Timer -= Time.deltaTime;

			if(m_Timer <= 0.0f)
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}

	public void ReceiveEvent(GameEvent gameEvent, GameObject target, int value)
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
			OnEnemyKilled(target, value);
			break;

		case GameEvent.e_CivilianKilled:
			OnCivilianKilled(target, value);
			break;

		case GameEvent.e_PlayerHit:
			OnPlayerHit(value);
			break;

		case GameEvent.e_PlayerKilled:
			OnPlayerKilled();
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

	void OnEnemyKilled(GameObject target, int score)
	{
		if(!m_GamePaused && (m_CurrentState == GameState.e_GamePart1 || m_CurrentState == GameState.e_GamePart2))
		{
			m_PlayerScore.IncreaseScore((uint)score);

			BaseAI enemy = target.GetComponent<BaseAI>();

			// TODO: achievements !!!
			/*if(enemy is Pelican)
			{

			}
			else if(enemy is Mini_UFO)
			{
				
			}
			else if(enemy is Boss_UFO)
			{
				
			}*/
		}
	}

	void OnCivilianKilled(GameObject target, int score)
	{
		if(!m_GamePaused && (m_CurrentState == GameState.e_GamePart1 || m_CurrentState == GameState.e_GamePart2))
		{
			m_PlayerScore.DecreaseScore((uint)score);
		}
	}

	void OnPlayerKilled()
	{		
		m_Timer = m_TimeBeforeReset;
	}

	void OnPlayerHit(int damage)
	{
		m_PlayerHealth.Hit(damage);
	}

	[ContextMenu("Move One State")]
	void MoveOneState()
	{
		m_CurrentState = (GameState)((int) m_CurrentState + 1);
	}

	[ContextMenu("Pause game")]
	void PauseGame()
	{
		ReceiveEvent(GameEvent.e_GamePaused, null, 0);
	}

	[ContextMenu("Hit player")]
	void HitPlayer()
	{
		m_PlayerHealth.Hit (10.0f);
	}

	[ContextMenu("Increase Score")]
	void IncreaseScore()
	{
		m_PlayerScore.IncreaseScore(int.MaxValue);
	}

	[ContextMenu("Decrease Score")]
	void DecreaseScore()
	{
		m_PlayerScore.DecreaseScore(30);
	}
}
