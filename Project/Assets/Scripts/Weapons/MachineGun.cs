using UnityEngine;
using System.Collections;

public class MachineGun : Weapon
{
    public KeyCode FireKey;
    public string FireButton;
	
	// Update is called once per frame
	protected override void update ()
    {
		if(Input.GetKey(FireKey))
		{
			Fire(transform.forward);
		}

        /*if (Time.timeScale != 0.0f && ((FireKey != KeyCode.None && Input.GetKey(FireKey)) || (FireButton != "" && Input.GetButton(FireButton))))
        {
			for(int i = 0; i < 6; i++)
			{
				Fire(transform.forward);
			}
        }*/
		else
		{
			EndFire();
		}
	}
}
