using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using LitJson;

public enum GameEvent
{
	e_GameLaunched,
	e_GamePaused,
	e_EnemyKilled,
	e_CivilianKilled,
	e_PlayerHit,
	e_PlayerKilled,
	e_EngagedBoss,
	e_GameExited
}

public class GameEventManager : MonoBehaviour 
{
	////////////////////////////////////////////
	// BrainCloud integration code
	////////////////////////////////////////////
	
	private void ReadStatistics()
	{
		// Ask brainCloud for statistics
		BrainCloudWrapper.GetBC().PlayerStatisticsService.ReadAllPlayerStats(StatsSuccess_Callback, StatsFailure_Callback, null);
	}
	
	private void SaveStatisticsToBrainCloud()
	{
		// Build the statistics name/inc value dictionary
		Dictionary<string, object> stats = new Dictionary<string, object> {
			{"BirdsShot", m_BirdsShot},
			{"MiniUFOsShot", m_MiniUFOsShot},
			{"CarsDestroyed", m_CarsDestroyed},
			{"BossesKilled", m_BossesKilled},
			{"TrucksDestroyed", m_TrucksDestroyed}
		};
		
		// Send to the cloud
		BrainCloudWrapper.GetBC().PlayerStatisticsService.IncrementPlayerStats(
			stats, StatsSuccess_Callback, StatsFailure_Callback, null);

		m_BirdsShot = 0;
		m_CarsDestroyed = 0;
		m_MiniUFOsShot = 0;
		m_TrucksDestroyed = 0;
		m_BossesKilled = 0;

		if(m_IsQuitting)
		{
			Application.Quit ();
		}
	}
	
	private void StatsSuccess_Callback(string responseData, object cbObject)
	{
		// Read the json and update our values
		JsonData jsonData = JsonMapper.ToObject (responseData);
		JsonData entries = jsonData["data"]["statistics"];

		m_BirdsShotGlobal = long.Parse(entries["BirdsShot"].ToString());
		m_CarsDestroyedGlobal = long.Parse(entries["CarsDestroyed"].ToString());
		m_MiniUFOsShotGlobal = long.Parse(entries["MiniUFOsShot"].ToString());
		m_TrucksDestroyedGlobal = long.Parse(entries["TrucksDestroyed"].ToString());
		m_BossesKilledGlobal = long.Parse(entries["TrucksDestroyed"].ToString());
	}
	
	private void StatsFailure_Callback(int statusCode, int reasonCode, string statusMessage, object cbObject)
	{
		Debug.Log (statusMessage);
	}



	public void GetLeaderboard(string aLeaderboardID)
	{
		m_LeaderboardReady = false;
		BrainCloudWrapper.GetBC().SocialLeaderboardService.GetGlobalLeaderboard(aLeaderboardID, BrainCloud.BrainCloudSocialLeaderboard.FetchType.HIGHEST_RANKED, 100, LeaderboardSuccess_Callback, LeaderboardFailure_Callback, null);
	}
	
	public void GetLeaderboardPage(string aLeaderboardID, int aIndex, int aSecondIndex)
	{
		m_LeaderboardReady = false;
		BrainCloudWrapper.GetBC().SocialLeaderboardService.GetGlobalLeaderboardPage(aLeaderboardID, BrainCloud.BrainCloudSocialLeaderboard.SortOrder.HIGH_TO_LOW, aIndex, aSecondIndex, true, LeaderboardSuccess_Callback, LeaderboardFailure_Callback, null);
	}

	public void LeaderboardSuccess_Callback(string responseData, object cbObject)
	{
		m_LeaderboardReady = true;
		m_LeaderboardData = JsonMapper.ToObject(responseData)["data"];
	}
	
	public void LeaderboardFailure_Callback(int a, int b, string responseData, object cbObject)
	{
		Debug.LogError(responseData);
	}
	
	public void SubmitLeaderboardData(int aKills, int aBombHits, int aDeaths)
	{
		BrainCloudWrapper.GetBC().SocialLeaderboardService.PostScoreToLeaderboard("Global Leaderboard", m_PlayerScore.Score, BrainCloudWrapper.GetBC().GetSessionId());
	}
	////////////////////////////////////////////


	// Statictics for achievements
	long m_BirdsShot;
	long m_CarsDestroyed;
	long m_MiniUFOsShot;
	long m_TrucksDestroyed;
	long m_BossesKilled;

	long m_BirdsShotGlobal;
	long m_CarsDestroyedGlobal;
	long m_MiniUFOsShotGlobal;
	long m_TrucksDestroyedGlobal;
	long m_BossesKilledGlobal;

	long m_BirdsShotAchievement = 100;
	long m_CarsDestroyedAchievement = 20;
	long m_MiniUFOsShotAchievement = 50;
	long m_TrucksDestroyedAchievement = 10;
	long m_BossesKilledAchievement = 5;


	bool m_IsQuitting = false;

	bool m_LeaderboardReady;
	public JsonData m_LeaderboardData;

	float m_TimeBeforeReset = 5.0f;
	float m_Timer;

	GameState m_CurrentState = GameState.e_MainMenu;

	bool m_GamePaused = false;

	public HealthManager m_PlayerHealth;
	public ScoreManager m_PlayerScore;
	public AchievementsManager m_AchievementsManager;

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

	void Start()
	{
		Time.timeScale = 0.0f;

		ReadStatistics();
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

		case GameEvent.e_EngagedBoss:
			OnBossEngaged();
			break;

		case GameEvent.e_GameExited:
			m_IsQuitting = true;
			SaveStatisticsToBrainCloud();
			break;
		}
	}

	void OnGameLaunched()
	{
		if(m_CurrentState == GameState.e_MainMenu)
		{
			m_CurrentState = GameState.e_GamePart1;

			Time.timeScale = 1.0f;
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

			List<AchievementType> types = new List<AchievementType>();
			long oldValue = 0;
			long newValue = 0;

			if(enemy is Pelican)
			{
				oldValue = m_BirdsShot;
				m_BirdsShot++;
				newValue = m_BirdsShot;

				types.Add(AchievementType.e_TrueFriend);
			}
			else if(enemy is Mini_UFO)
			{
				oldValue = m_MiniUFOsShot;
				m_MiniUFOsShot++;
				newValue = m_MiniUFOsShot;
				
				types.Add(AchievementType.e_LastProtector);
			}
			else if(enemy is Boss_UFO)
			{
				m_CurrentState = GameState.e_End;

				m_BossesKilled++;

				oldValue = m_BossesKilled;
				m_BossesKilled++;
				newValue = m_BossesKilled;
				
				types.Add(AchievementType.e_WillSmith);
				types.Add(AchievementType.e_StillTrying);
			}

			foreach(AchievementType type in types)
			{
				UpdateAchievements(type, oldValue, newValue);
			}
		}
	}

	void OnCivilianKilled(GameObject target, int score)
	{
		if(!m_GamePaused && (m_CurrentState == GameState.e_GamePart1 || m_CurrentState == GameState.e_GamePart2))
		{
			m_PlayerScore.DecreaseScore((uint)score);

			/*if(target.tag == "Car")
			{

			}*/
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

	void OnBossEngaged()
	{
		m_CurrentState = GameState.e_GamePart2;
	}

	void UpdateAchievements(AchievementType type, long previousValue, long newValue)
	{
		if(IsAchievementJustUnlocked(type, previousValue, newValue))
		{
			m_AchievementsManager.ShowAchievement(type);
		}
	}

	bool IsAchievementJustUnlocked(AchievementType type, long previousValue, long newValue)
	{
		bool achievementUnlocked = false;

		switch(type)
		{
		case AchievementType.e_DrunkDriver:
			if(previousValue + m_CarsDestroyedGlobal< m_CarsDestroyedAchievement
			   && newValue + m_CarsDestroyedGlobal >= m_CarsDestroyedAchievement)
			{
				achievementUnlocked = true;
			}
			break;
			
		case AchievementType.e_FourthOfJuly:
			if(previousValue + m_TrucksDestroyedGlobal < m_TrucksDestroyedAchievement
			   && newValue + m_TrucksDestroyedGlobal >= m_TrucksDestroyedAchievement)
			{
				achievementUnlocked = true;
			}
			break;
			
		case AchievementType.e_LastProtector:
			if(previousValue + m_MiniUFOsShotGlobal< m_MiniUFOsShotAchievement
			   && newValue + m_MiniUFOsShotGlobal >= m_MiniUFOsShotAchievement)
			{
				achievementUnlocked = true;
			}
			break;
			
		case AchievementType.e_StillTrying:
			if(previousValue + m_BossesKilledGlobal < m_BossesKilledAchievement
			   && newValue + m_BossesKilledGlobal >= m_BossesKilledAchievement)
			{
				achievementUnlocked = true;
			}
			break;
			
		case AchievementType.e_TrueFriend:
			if(previousValue + m_BirdsShotGlobal < m_BirdsShotAchievement
			   && newValue + m_BirdsShotGlobal >= m_BirdsShotAchievement)
			{
				achievementUnlocked = true;
			}
			break;
			
		case AchievementType.e_WillSmith:
			if(previousValue + m_BossesKilledGlobal < 1
			   && newValue + m_BossesKilledGlobal >= 1)
			{
				achievementUnlocked = true;
			}
			break;
		}

		return achievementUnlocked;
	}

	public bool IsAchievementUnlocked(AchievementType type)
	{
		bool achievementUnlocked = false;

		switch(type)
		{
		case AchievementType.e_DrunkDriver:
			if(m_CarsDestroyed + m_CarsDestroyedGlobal >= m_CarsDestroyedAchievement)
			{
				achievementUnlocked = true;
			}
			break;
			
		case AchievementType.e_FourthOfJuly:
			if(m_TrucksDestroyed + m_TrucksDestroyedGlobal >= m_TrucksDestroyedAchievement)
			{
				achievementUnlocked = true;
			}
			break;
			
		case AchievementType.e_LastProtector:
			if(m_MiniUFOsShot + m_MiniUFOsShotGlobal >= m_MiniUFOsShotAchievement)
			{
				achievementUnlocked = true;
			}
			break;
			
		case AchievementType.e_StillTrying:
			if(m_BossesKilled + m_BossesKilledGlobal >= m_BossesKilledAchievement)
			{
				achievementUnlocked = true;
			}
			break;
			
		case AchievementType.e_TrueFriend:
			if(m_BirdsShot + m_BirdsShotGlobal >= m_BirdsShotAchievement)
			{
				achievementUnlocked = true;
			}
			break;
			
		case AchievementType.e_WillSmith:
			if(m_BossesKilled + m_BossesKilledGlobal >= 1)
			{
				achievementUnlocked = true;
			}
			break;
		}

		return achievementUnlocked;
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

	[ContextMenu("Unlock true friend")]
	void UnlockTrueFriend()
	{
		m_BirdsShot = 130;
		UpdateAchievements(AchievementType.e_TrueFriend, 0, m_BirdsShot);
	}
}
