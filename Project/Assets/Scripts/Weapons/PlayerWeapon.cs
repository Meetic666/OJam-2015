using UnityEngine;
using System.Collections;

public class PlayerWeapon : Weapon
{
    public KeyCode FireKey;
    public string FireButton;
	
	// Update is called once per frame
	protected override void update ()
    {
        if (Input.GetKey(FireKey) || Input.GetButton(FireButton))
        {
            Fire();
        }
	}
}
