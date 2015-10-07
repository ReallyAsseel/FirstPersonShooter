using UnityEngine;
using System.Collections;

public class GunMovement : MonoBehaviour {

    public Vector3 ADSPosition, StillPosition, wantedPosition, firePosition;
	public bool isFiring;
	public GunMechanics bulletControl;
    Animator anim;

	void Start () {
		isFiring = false;
		wantedPosition = transform.localPosition;
		bulletControl = GetComponentInChildren<GunMechanics> ();
        anim = this.gameObject.GetComponent<Animator>();
	}
	
	void Update () {
		transform.localPosition = Vector3.Lerp (transform.localPosition, wantedPosition, bulletControl.ADSsmoothness/6f);
		firePosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, bulletControl.Recoil.x);
   
        if(bulletControl.isReloading)
        {
            anim.SetBool("Reload", true);
        } else
        {
            anim.SetBool("Reload", false);
        }

        if (Input.GetMouseButton (1)) {
			wantedPosition = ADSPosition;
			if(Input.GetMouseButtonDown(0)) {
				isFiring = true;
			} else { 
				isFiring = false;
			}
		} else if(Input.GetMouseButtonDown(0)){
			isFiring = true;
		} else {
			wantedPosition = StillPosition;
		}
	}
}
