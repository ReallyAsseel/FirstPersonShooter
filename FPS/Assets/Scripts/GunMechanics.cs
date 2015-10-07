using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunMechanics : MonoBehaviour {
	public float bulletSpeed, CurrentRate, RateOfFire, ADSsmoothness, ReloadTime, Range, Damage, CurrentReloadRate, accuracy;
	public int MagazineSize, NumberOfMagazines, rand, currentBullets, barrelCapacity;
    public GameObject bulletSpawn, Muzzle, PrimaryGunSlot, SecondaryGunSlot;
	public Vector3 Recoil;
    public Camera camera;
	public bool ADS, isAutomatic, isReloading, OutOfAmmo, MagazineDropped, isShotgun;
	public GunMovement gunController;
    public PlayerController playerController;
    public Crosshairs crossHairs;

	// Use this for initialization
	void Start () {
		bulletSpawn = GameObject.Find("BulletSpawn");
        PrimaryGunSlot = GameObject.Find("GunHolder");
        SecondaryGunSlot = GameObject.Find("SecondGun");
        playerController = GameObject.FindObjectOfType<PlayerController>();
		Muzzle = GameObject.Find("Muzzle");
		OutOfAmmo = false;
		gunController = GetComponentInParent<GunMovement> ();
        camera = GameObject.FindObjectOfType<Camera>();
        ADS = false;
		isReloading = false;
        OutOfAmmo = false;
        MagazineDropped = false;
        isShotgun = false;
		CurrentRate = 0f;
		CurrentReloadRate = 0f;
		InitializeGuns();
        crossHairs = GameObject.Find("CH").GetComponent<Crosshairs>();
    }

    void InitializeGuns()
    {
        switch (playerController.currentWeapon.name)
            {
                case "deagle":
                    bulletSpeed = 250.0f;
                    barrelCapacity = 1;
                    RateOfFire = 0.2f;
                    Damage = 75.0f;
                    MagazineSize = 8;
                    NumberOfMagazines = 5;
                    ReloadTime = 1.5f;
                    ADSsmoothness = 2f;
                    currentBullets = 8;
                    isAutomatic = false;
                    Recoil = new Vector3(-1.5f, -20f, 0.01f); //x is recoil in z, y is recoil in x axis, z is time
                    accuracy = 15f;
                    setCrossHairs(accuracy);
                    break;
                case "SPAS 12":
                    bulletSpeed = 250.0f;
                    barrelCapacity = 3;
                    RateOfFire = 0.5f;
                    Damage = 25.0f;
                    MagazineSize = 8;
                    NumberOfMagazines = 5;
                    ReloadTime = 0.7f;
                    ADSsmoothness = 1.5f;
                    currentBullets = 8;
                    isAutomatic = false;
                    isShotgun = true;
                    accuracy = 40f;
                    setCrossHairs(accuracy);
                    Recoil = new Vector3(-2.5f, -20f, 0.1f);

                    break;
            }
    }

	// Update is called once per frame
	void Update () {
            gunReady();
            AimDownSights();
            Reload();
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
            //crossHairs.ToggleCrossHairs(false);
            ////anim.SetBool("ADS", true);
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
           // crossHairs.ToggleCrossHairs(true);
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

    void setCrossHairs(float accuracy)
    {
        GameObject.Find("CH").GetComponent<Crosshairs>().Radius = accuracy;
    }

	void Reload()
    {
		//Animation play
		if (isReloading)
        {
			if (CurrentReloadRate < ReloadTime)
            {
                DropMagazine();
                CurrentReloadRate += 1 * Time.deltaTime;
			} else { //Actual reloading takes place here.
				if(NumberOfMagazines != 0) {
                    MagazineDropped = false;
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

    void DropMagazine() {
        if (!MagazineDropped)
        {
            GameObject Magazine = (GameObject)Instantiate(Resources.Load("Prefabs/Magazine"), Vector3.zero, Quaternion.identity);
            Magazine.transform.position = GameObject.Find("MagazineSpawn").transform.position;
            Magazine.GetComponent<Rigidbody>().AddTorque(50, 0f, 50f);
            MagazineDropped = true;
            GameObject.Destroy(Magazine, 5f);
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
				float a = Random.Range(0.2f, 0.5f);
				if(gunController.isFiring) {
					gunController.wantedPosition = gunController.firePosition;
				}
				Muzzle.transform.localScale = new Vector3(a, a, a);
				Muzzle.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Muzzle" + rand);
				Muzzle.GetComponent<SpriteRenderer>().enabled = true;

                if (!isShotgun)
                {
                    GameObject bulletclone;
                    bulletclone = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/bullet"), transform.position, transform.rotation);
                    bulletclone.transform.position = bulletSpawn.transform.position;
                    bulletclone.GetComponent<Rigidbody>().velocity = transform.parent.forward * bulletSpeed;
                } else if(isShotgun)
                {
                    GameObject[] bulletclone = new GameObject[barrelCapacity];
                    for (int i = 0; i < bulletclone.Length; i++)
                    {
                        bulletclone[i] = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/bullet"), transform.position, transform.rotation);
                        bulletclone[i].transform.position = bulletSpawn.transform.position;
                    }
                    bulletclone[0].GetComponent<Rigidbody>().velocity = transform.parent.forward * bulletSpeed;
                    bulletclone[1].GetComponent<Rigidbody>().velocity = transform.parent.forward * bulletSpeed + new Vector3(0.8f, 0.3f, 0f);
                    bulletclone[2].GetComponent<Rigidbody>().velocity = transform.parent.forward * bulletSpeed + new Vector3(-0.9f, 0.3f, 0f);

                }
            } else {
				isReloading = true;
			}
        }
    }
}
