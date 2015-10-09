using UnityEngine;
using System.Collections;

public class GunMovement : MonoBehaviour {

    public Vector3 ADSPosition, StillPosition, wantedPosition, firePosition;
    public bool isFiring, isPickup;
	public GunMechanics bulletControl;
    public PlayerController playerController;
    Animator anim;

	void Start () {
		wantedPosition = transform.localPosition;
		bulletControl = GetComponentInChildren<GunMechanics> ();
        anim = this.gameObject.GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
        Still();
		if(this.gameObject.tag == "Pickup") {
			isPickup = true;
		}
	}
	
	void Update () {
       // if (this.gameObject.name == playerController.currentWeapon.name)
       // {
		if (!isPickup) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, wantedPosition, bulletControl.ADSsmoothness / 6f);
			firePosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, bulletControl.Recoil.x);
			if (bulletControl.isReloading) {
				anim.SetBool ("Reload", true);
			} else {
				anim.SetBool ("Reload", false);
			}
			
			Gunmovement ();
		} else {
			PickupAnimation ();
		}
       // }
    }

    public void Recoil()
    {
        wantedPosition = firePosition;
    }

    public void Still()
    {
        wantedPosition = StillPosition;   
    }

    public void ADS()
    {
        wantedPosition = ADSPosition;
    }

	public void PickupAnimation() {
		anim.SetBool ("Pickup", true);
	}

    void Gunmovement()
    {
        if (Input.GetMouseButton(1))
        {
           // wantedPosition = ADSPosition;
			if(!this.gameObject.GetComponentInChildren<GunMechanics>().isAutomatic) {
	            if (Input.GetMouseButtonDown(0))
	            {
	                isFiring = true;
	            }
	            else
	            {
	                isFiring = false;
	            }
			} else {
				isFiring = Input.GetMouseButton(0);
			}
        }
		else if (this.gameObject.GetComponentInChildren<GunMechanics>() != null && !this.gameObject.GetComponentInChildren<GunMechanics>().isAutomatic)
		{
			if(Input.GetMouseButtonDown(0)) {
				isFiring = true;
			} else {
				isFiring = false;
			}
		} else if(this.gameObject.GetComponentInChildren<GunMechanics>() != null && this.gameObject.GetComponentInChildren<GunMechanics>().isAutomatic) {
			isFiring = Input.GetMouseButton(0);
		}
	}
}
