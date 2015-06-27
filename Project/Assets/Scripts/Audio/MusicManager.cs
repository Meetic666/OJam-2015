using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour 
{
	public List<AudioClip> m_AudioClips;

	AudioSource m_Source;

	GameEventManager m_GameEventManager;

	enum MusicState
	{
		e_Intro,
		e_MenuLoop,
		e_BridgeToGame,
		e_GameLoop1,
		e_GameBridge,
		e_GameLoop2,
		e_GameEnd
	}

	MusicState m_CurrentState = MusicState.e_Intro;
	MusicState m_StateToAchieve = MusicState.e_Intro;

	// Use this for initialization
	void Start () 
	{
		m_Source = gameObject.AddComponent<AudioSource>();

		m_Source.clip = m_AudioClips[(int) m_CurrentState];
		m_Source.loop = false;
		m_Source.Play();

		m_GameEventManager = FindObjectOfType<GameEventManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateMusicState();

		UpdateMusic();
	}

	void UpdateMusicState()
	{		
		switch(m_GameEventManager.CurrentState)
		{
		case GameState.e_MainMenu:
			m_StateToAchieve = MusicState.e_MenuLoop;
			break;

		case GameState.e_GamePart1:
			m_StateToAchieve = MusicState.e_GameLoop1;
			break;

		case GameState.e_GamePart2:
			m_StateToAchieve = MusicState.e_GameLoop2;
			break;

		case GameState.e_End:
			m_StateToAchieve = MusicState.e_GameEnd;
			break;
		}
	}

	void UpdateMusic()
	{	
		if(!m_Source.isPlaying)
		{
			if((int) m_CurrentState < (int) m_StateToAchieve)
			{
				m_CurrentState = (MusicState)((int) m_CurrentState + 1);

				m_Source.clip = m_AudioClips[(int) m_CurrentState];
			}
			
			m_Source.Play();
		}
	}
}
