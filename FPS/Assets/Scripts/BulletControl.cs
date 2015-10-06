using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BulletControl : MonoBehaviour {
	public float bulletSpeed, CurrentRate, RateOfFire, ADSsmoothness, Recoil, ReloadTime, Range, Damage, CurrentReloadRate;
	public int MagazineSize, NumberOfMagazines, rand, currentBullets;
	public GameObject spawn, bulletclone, Muzzle;
	public Vector3 offSet;
    public Camera camera;
	public bool ADS, isAutomatic, isReloading, OutOfAmmo;
	public GunController gunController;

	// Use this for initialization
	void Start () {
		spawn = GameObject.Find("BulletSpawn");
		Muzzle = GameObject.Find("Muzzle");
		OutOfAmmo = false;
		gunController = GetComponentInParent<GunController> ();
        camera = GameObject.FindObjectOfType<Camera>();
        ADS = false;
		isReloading = false;
		CurrentRate = 0f;
		CurrentReloadRate = 0f;
		InitializeGuns ();
    }

	void InitializeGuns() {
		switch (gameObject.name)
		{
		case "deagleModel":
			bulletSpeed = 250.0f;
			RateOfFire = 0.2f;
			Damage = 75.0f;
			MagazineSize = 8;
			NumberOfMagazines = 5;
			ReloadTime = 1.5f;
			ADSsmoothness = 2f;
			currentBullets = 8;
			isAutomatic = false;
			break;
		case "SPAS 12Model":
			bulletSpeed = 250.0f;
			RateOfFire = 0.5f;
			Damage = 25.0f;
			MagazineSize = 8;
			NumberOfMagazines = 5;
			ReloadTime = 0.7f;
			ADSsmoothness = 1.5f;
			currentBullets = 8;
			isAutomatic = false;
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		gunReady();
		AimDownSights();
		Reload ();
	}

	void gunReady() {
		//RECOIL
		if(CurrentRate >= RateOfFire) {
			if (GameObject.FindGameObjectsWithTag("Weapon").Length != 0)
			{
				Fire();
			}
		} else {
			CurrentRate += 1f * Time.deltaTime;
			Muzzle.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	void AimDownSights()
	{
		if (Input.GetMouseButton(1))
		{
			ADS = true;
			////anim.SetBool("ADS", true);
			GameObject.Find("Crosshair").GetComponent<Image>().enabled = false;
			if (camera.fieldOfView < 29)
			{
				camera.fieldOfView += ADSsmoothness;
			}
			else if (camera.fieldOfView > 31)
			{
				camera.fieldOfView -= ADSsmoothness;
			}
		}
		else
		{
			ADS = false;
			GameObject.Find("Crosshair").GetComponent<Image>().enabled = true;
			//anim.SetBool("ADS", false);
			if (camera.fieldOfView < 59)
			{
				camera.fieldOfView += ADSsmoothness;
			}
			else if (camera.fieldOfView > 61)
			{
				camera.fieldOfView -= ADSsmoothness;
			}
		}
	}

	void Reload() {
		//Animation play
		if (isReloading) {
			if (CurrentReloadRate < ReloadTime) {
				CurrentReloadRate += 1 * Time.deltaTime;
			} else {
				if(NumberOfMagazines != 0) {
					NumberOfMagazines--;
					currentBullets = MagazineSize;
					CurrentReloadRate = 0;
					isReloading = false;
				} else {
					OutOfAmmo = true;
				}
			}
		}
	}

    void Fire()
    {
		//SHOOTING
		if (Input.GetMouseButtonDown(0))
        {
			if(currentBullets != 0) {
				currentBullets--;
				CurrentRate = 0;
				rand = Random.Range(1, 3);
				float a = Random.Range(0.5f, 1.5f);
				if(gunController.isFiring) {
					gunController.wantedPosition = gunController.firePosition;
				}
				Muzzle.transform.localScale = new Vector3(a, a, a);
				Muzzle.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Muzzle" + rand);
				Muzzle.GetComponent<SpriteRenderer>().enabled = true;
				bulletclone = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/bullet"), transform.position, transform.rotation);
				bulletclone.transform.position = spawn.transform.position;
				bulletclone.GetComponent<Rigidbody>().velocity = transform.parent.forward * bulletSpeed;
			} else {
				isReloading = true;
			}
        }
    }
}
