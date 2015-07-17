using UnityEngine;
using System.Collections;

public class RocketLauncher : Weapon {

	public KeyCode FireKey;
	public string FireButton;

	public Vector3[] m_FiringLocations = new Vector3[6];
	
	// Update is called once per frame
	protected override void update ()
	{
		if (CanFire() && Time.timeScale != 0.0f && ((FireKey != KeyCode.None && Input.GetKey(FireKey)) || (FireButton != "" && Input.GetButton(FireButton))))
		{
			FiringSpeed = 0;

			for(int i = 0; i < m_FiringLocations.Length; i++)
			{
				if(m_FiringLocations[i] != null)
				{
					if(i == m_FiringLocations.Length - 1)
					{
						FiringSpeed = 2;
					}

					Fire(m_FiringLocations[i] + transform.forward, transform.forward);
				}
			}
		}
		else
		{
			EndFire();
		}
	}
}
