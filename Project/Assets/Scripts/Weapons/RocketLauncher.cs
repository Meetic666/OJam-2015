using UnityEngine;
using System.Collections;

public class RocketLauncher : Weapon {

	public KeyCode FireKey;
	public string FireButton;

	public Vector3[] m_FiringLocations = new Vector3[6];
	
	// Update is called once per frame
	protected override void update ()
	{
		if (Time.timeScale != 0.0f && ((FireKey != KeyCode.None && Input.GetKey(FireKey)) || (FireButton != "" && Input.GetButton(FireButton))))
		{
			for(int i = 0; i < m_FiringLocations.Length; i++)
			{
				if(m_FiringLocations[i] != null)
				{
					Fire(m_FiringLocations[i] + transform.forward);
				}
			}
			FiringSpeed = 2;
		}
		else
		{
			EndFire();
		}
	}
}
