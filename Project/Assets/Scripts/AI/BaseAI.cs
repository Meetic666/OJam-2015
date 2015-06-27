using UnityEngine;
using System.Collections;

public class BaseAI : MonoBehaviour 
{
	public int Health
	{
		get;
		set;
	}

	public float MovementSpeed
	{
		get;
		set;
	}

	public float MinDist2Player 
	{
		get;
		set;
	}

	void Awake()
	{
		Health = 1;
	}
}
