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
			Vector3 direction = Vector3.forward;

			direction.x = Random.Range(-RandomOffset, RandomOffset) / 5.0f;
			direction.y = Random.Range (-RandomOffset, RandomOffset) / 5.0f;

			direction = transform.TransformDirection(direction);

			Fire(transform.forward, direction);
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
