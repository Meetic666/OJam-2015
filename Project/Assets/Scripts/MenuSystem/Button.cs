using UnityEngine;
using System.Collections;

public enum ActionType
{
	e_AnyKey,
	e_Start,
	e_Quit,
	e_Achievements,
	e_Leaderboard,
	e_Resume,
	e_BackToMenu,
	e_BackToPause
}

public class Button : MonoBehaviour 
{
	public ActionType m_Action;

	MenuManager m_MenuManager;

	// Use this for initialization
	void Start () 
	{
		m_MenuManager = FindObjectOfType<MenuManager>();
	}

	void OnMouseDown()
	{
		m_MenuManager.DoAction(m_Action);
	}
}
