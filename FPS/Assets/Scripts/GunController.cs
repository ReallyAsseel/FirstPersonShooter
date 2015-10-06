using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	public Vector3 ADSPosition, StillPosition, wantedPosition, firePosition;
	public bool isFiring;
	public BulletControl bulletControl;

	void Start () {
		isFiring = false;
		wantedPosition = transform.localPosition;
		bulletControl = GetComponentInChildren<BulletControl> ();
	}
	
	void Update () {
		transform.localPosition = Vector3.Lerp (transform.localPosition, wantedPosition, bulletControl.ADSsmoothness/6f);

		firePosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, -1.5f);

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
