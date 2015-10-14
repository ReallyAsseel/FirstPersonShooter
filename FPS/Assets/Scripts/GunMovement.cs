using UnityEngine;
using System.Collections;

public class GunMovement : MonoBehaviour {

    public Vector3 ADSPosition, StillPosition, wantedPosition, firePosition;
    public bool isFiring, isPickup, reloadAnim;
	public GunMechanics gunMech;
    public PlayerController playerController;
    public Animator anim;

	void Start () {
		if (this.gameObject.tag == "Pickup") {
			isPickup = true;
		} else {
			isPickup = false;
		}
		reloadAnim = false;
		wantedPosition = transform.localPosition;
		gunMech = this.gameObject.GetComponentInChildren<GunMechanics> ();
		anim = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
        Still();

	}
	
	void Update () {
		if (!isPickup && gunMech != null) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, wantedPosition, gunMech.ADSsmoothness / 6f);
			firePosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, gunMech.Recoil.x);
			if (gunMech.isReloading) {
				anim.SetBool ("Reload", true);
			} else {
				anim.SetBool("Reload", false);
			}
			Gunmovement ();
		} else {
			PickupAnimation ();
		}
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

	public void isDoneReloading() {
	//	reloadAnim = false;
	}

    void Gunmovement()
    {
        if (Input.GetMouseButton(1))
        {
			if(gunMech != null && !gunMech.isAutomatic) {
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
		else if (gunMech != null && !gunMech.isAutomatic)
		{
			if(Input.GetMouseButtonDown(0)) {
				isFiring = true;
			} else {
				isFiring = false;
			}
		} else if(gunMech != null && gunMech.isAutomatic) {
			isFiring = Input.GetMouseButton(0);

		}
	}
}
