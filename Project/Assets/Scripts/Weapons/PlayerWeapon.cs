using UnityEngine;
using System.Collections;

public class PlayerWeapon : Weapon
{
    public KeyCode FireKey;
    public string FireButton;
	
	// Update is called once per frame
	protected override void update ()
    {
        if (Time.timeScale != 0.0f && ((FireKey != KeyCode.None && Input.GetKey(FireKey)) || (FireButton != "" && Input.GetButton(FireButton))))
        {
            Fire();
        }
		else
		{
			EndFire();
		}
	}
}
