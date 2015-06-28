using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour 
{
	public SpriteRenderer[] m_DigitRenderers;

	public Sprite[] m_DigitSprites;

	uint m_Score;
	
	public void IncreaseScore(uint scoreAmount)
	{
		m_Score += scoreAmount;

		SetUpRenderers();
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
		
		SetUpRenderers();
	}

	void SetUpRenderers()
	{
		uint score = m_Score;

		for(int i = m_DigitRenderers.Length - 1; i >= 0; i--)
		{
			uint digit = score / (uint)Mathf.Pow (10, i);

			m_DigitRenderers[i].sprite = m_DigitSprites[digit];

			score -= digit * (uint)Mathf.Pow (10, i);
		}
	}
}
