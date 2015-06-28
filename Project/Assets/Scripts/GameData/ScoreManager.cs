using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour 
{
	public SpriteRenderer[] m_DigitRenderers;

	public Sprite[] m_DigitSprites;

	public float m_SpeedOfUpdate = 1.0f;

	uint m_Score;

	float m_CurrentScore;

	public AudioClip m_CoinSound;
	AudioSource m_Source;

	void Start()
	{
		m_Source = gameObject.AddComponent<AudioSource>();

		m_Source.clip = m_CoinSound;
		m_Source.loop = true;
		m_Source.playOnAwake = false;
	}

	void Update()
	{
		if(m_CurrentScore != m_Score)
		{
			m_CurrentScore = Mathf.Lerp(m_CurrentScore, (float)m_Score, m_SpeedOfUpdate * Time.deltaTime);

			float result = Mathf.Abs (m_CurrentScore - (float)m_Score);

			if(result <= 5.0f)
			{
				m_Source.Stop();

				m_CurrentScore = m_Score;
			}

			SetUpRenderers();
		}
	}
	
	public void IncreaseScore(uint scoreAmount)
	{
		if(m_Score <= uint.MaxValue - scoreAmount)
		{
			m_Score += scoreAmount;
		}
		else
        {
            m_Score = uint.MaxValue;
        }

		m_Source.Play();
	}

	public void DecreaseScore(uint scoreAmount)
	{
		if(m_Score >= scoreAmount)
		{
			m_Score -= scoreAmount;
		}
		else
		{
			m_Score = 0;
		}
	}

	void SetUpRenderers()
	{
		uint score = (uint) m_CurrentScore;

		for(int i = m_DigitRenderers.Length - 1; i >= 0; i--)
		{
			uint digit = score / (uint)Mathf.Pow (10, i);

			m_DigitRenderers[i].sprite = m_DigitSprites[digit];

			score -= digit * (uint)Mathf.Pow (10, i);
		}
	}
}
