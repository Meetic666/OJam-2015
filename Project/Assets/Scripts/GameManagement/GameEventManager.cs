﻿using UnityEngine;
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

		case GameEvent.e_CivilianKilled:
			OnCivilianKilled(target);
			break;

		case GameEvent.e_PlayerHit:
			OnPlayerHit(target);
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

	void OnEnemyKilled(GameObject target)
	{
		if(!m_GamePaused && (m_CurrentState == GameState.e_GamePart1 || m_CurrentState == GameState.e_GamePart2))
		{
			// TODO: increment score
		}
	}

	void OnCivilianKilled(GameObject target)
	{
		if(!m_GamePaused && (m_CurrentState == GameState.e_GamePart1 || m_CurrentState == GameState.e_GamePart2))
		{
			// TODO: decrement score
		}
	}

	void OnPlayerKilled()
	{		
		m_Timer = m_TimeBeforeReset;
	}

	void OnPlayerHit(GameObject projectile)
	{
		float damage = 0;

		if(projectile.GetComponent<Pelican>())
		{
			damage = projectile.GetComponent<Pelican>().AtkDamage;
		}
		else if(projectile.GetComponent<Bullet>())
		{
			// TODO: get bullet's damage
			//damage = projectile.GetComponent<Bullet>().
		}

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
		ReceiveEvent(GameEvent.e_GamePaused, null);
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
