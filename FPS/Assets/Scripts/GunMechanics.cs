using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunMechanics : MonoBehaviour {
	public float bulletSpeed, CurrentRate, RateOfFire, ADSsmoothness, ReloadTime, Range, Damage, CurrentReloadRate, accuracy, zoomIn;
	public int MagazineSize, NumberOfMagazines, rand, currentBullets, barrelCapacity;
    public GameObject Muzzle, PrimaryGunSlot, SecondaryGunSlot;
	public Vector3 Recoil;    
	public Transform bulletSpawn, magazineSpawn;
	public bool ADS, isAutomatic, isReloading, OutOfAmmo, MagazineDropped, isShotgun, canPickup, isSniper;
	public GunMovement gunController;
    public PlayerController playerController;
    public Crosshairs crossHairs;

	// Use this for initialization
	public void Start () {
		playerController = GameObject.FindObjectOfType<PlayerController>();
		if(this.gameObject.tag == "Weapon") {
			canPickup = false;
            Muzzle = GameObject.Find(this.gameObject.name + "Muzzle");
            bulletSpawn = GameObject.Find(this.gameObject.name + "BulletSpawn").transform;
            magazineSpawn = GameObject.Find(this.gameObject.name + "MagazineSpawn").transform;
            PrimaryGunSlot = GameObject.Find("GunHolder");
			SecondaryGunSlot = GameObject.Find("SecondGun");
			OutOfAmmo = false;
			gunController = GetComponentInParent<GunMovement> ();
			ADS = false;
			isReloading = false;
			OutOfAmmo = false;
			MagazineDropped = false;
			isShotgun = false;
			isSniper = false;
			isAutomatic = false;
			CurrentRate = 0f;
			CurrentReloadRate = 0f;
			InitializeGuns(this.gameObject.name);
			crossHairs = GameObject.Find("CH").GetComponent<Crosshairs>();
			//if(playerController.currentWeapon.name + "Model" == this.gameObject.name) {
				
			//}
        }
    }

    public void InitializeGuns(string name)
    {

        switch (name)
            {
                case "deagleModel":
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
					isShotgun = false;
                    accuracy = 15f;
					Recoil = new Vector3(-1.5f, -20f, 0.01f); //x is recoil in z, y is recoil in x axis, z is time
					setCrossHairs(accuracy);
                    zoomIn = 57f;
                    break;
                case "SPAS 12Model":
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
                    Recoil = new Vector3(-2.5f, -20f, 0.1f);
					setCrossHairs(accuracy);
                    zoomIn = 50f;
                    break;
				case "G36CModel":
					bulletSpeed = 250.0f;
					barrelCapacity = 1;
					RateOfFire = 0.1f;
					Damage = 35.0f;
					MagazineSize = 30;
					NumberOfMagazines = 5;
					ReloadTime = 0.7f;
					ADSsmoothness = 1.5f;
					currentBullets = 30;
					isAutomatic = true;
					isShotgun = false;
					accuracy = 50f;
					Recoil = new Vector3(-0.1f, -5f, 0.06f);
					setCrossHairs(accuracy);
                    zoomIn = 35f;
					break;
				case "DruganovModel":
					bulletSpeed = 250.0f;
					barrelCapacity = 1;
					RateOfFire = 1.5f;
					Damage = 100f;
					MagazineSize = 5;
					NumberOfMagazines = 5;
					ReloadTime = 1f;
					ADSsmoothness = 3f;
					currentBullets = 5;
					isAutomatic = false;
					isShotgun = false;
					isSniper = true;
					accuracy = 1f;
					Recoil = new Vector3(-3f, -20f, 0.5f);
					setCrossHairs(accuracy);
                    zoomIn = 5f;
					break;
            }
    }

	void Update () {
	    if(this.gameObject.tag == "Pickup")
        {
            if (Input.GetKey(KeyCode.F) && canPickup)
            {
                if (playerController.secondaryWeapon == null)
                {
                    playerController.AddWeapon("Gun", this.gameObject.transform.parent.name);
                    gameObject.GetComponentInParent<Animator>().Stop();
                    GameObject.Find("PickupText").GetComponent<Text>().enabled = false;
                    Destroy(gameObject);
                } else if(this.gameObject.name != playerController.secondaryWeapon.name + "Model")
                {
                    playerController.AddWeapon("Gun", this.gameObject.transform.parent.name);
                    gameObject.GetComponentInParent<Animator>().Stop();
                    GameObject.Find("PickupText").GetComponent<Text>().enabled = false;
                    Destroy(gameObject);
                }
            }
        }
	}


	void OnTriggerEnter(Collider collider) {
		if(this.gameObject.tag == "Pickup") {
			if(collider.tag == "Player") {
				//Display "press f to pick up"
				GameObject.Find("PickupText").GetComponent<Text>().enabled = true;
				canPickup = true;
			}
		}
	}

	void OnTriggerExit(Collider collider) {
		if(this.gameObject.tag == "Pickup") {
			if(collider.tag == "Player") {
				//Display "press f to pick up"
				GameObject.Find("PickupText").GetComponent<Text>().enabled = false;
				canPickup = false;
			}
		}
	}

	public void AimDownSights(Camera camera)
	{
        if (!isReloading)
        {
            ADS = true;
            crossHairs.ToggleCrossHairs(false);

            if (camera.fieldOfView < zoomIn - 1)
            {
                camera.fieldOfView += ADSsmoothness;
            }
            else if (camera.fieldOfView > zoomIn + 1)
            {
                camera.fieldOfView -= ADSsmoothness;
            }
        }
        
	}

	public void ReturnFromSights(Camera camera) 
	{
		ADS = false;
		crossHairs.ToggleCrossHairs(true);
		if (camera.fieldOfView < 59)
		{
			camera.fieldOfView += ADSsmoothness;
		}
		else if (camera.fieldOfView > 61)
		{
			camera.fieldOfView -= ADSsmoothness;
		}
	}

    public void setCrossHairs(float accuracy)
    {
        GameObject.Find("CH").GetComponent<Crosshairs>().Radius = accuracy;
    }

	public void Reload()
    {
		//Animation play
		if (isReloading)
        {
			if (CurrentReloadRate < ReloadTime)
            {
                DropMagazine();
                CurrentReloadRate += 1 * Time.deltaTime;
			} else { //Actual reloading takes place here.
				if(NumberOfMagazines > 0) {
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

    public void DropMagazine() {
        if (!MagazineDropped)
        {
            GameObject Magazine = (GameObject)Instantiate(Resources.Load("Prefabs/Magazine"), Vector3.zero, Quaternion.identity);

			if(this.gameObject.name == playerController.currentWeapon.name + "Model") {
                Debug.Log(this.gameObject.name);
                if (magazineSpawn != null)
                {
                    Magazine.transform.position = magazineSpawn.position;
                } else
                {
                    magazineSpawn = GameObject.Find(this.gameObject.name + "MagazineSpawn").transform;
                }
			}
            Magazine.GetComponent<Rigidbody>().AddTorque(50, 0f, 50f);
            MagazineDropped = true;
            GameObject.Destroy(Magazine, 5f);
        }
    }

    public void Fire()
    {
	//SHOOTING
		if(currentBullets != 0 && !OutOfAmmo) {
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
				bulletclone.transform.Rotate(90f, 0f, 0f);
				bulletclone.transform.position = bulletSpawn.position;
				bulletclone.GetComponent<Rigidbody>().velocity = transform.parent.forward * bulletSpeed;
			} else if(isShotgun)
			{
				GameObject[] bulletclone = new GameObject[barrelCapacity];
				for (int i = 0; i < bulletclone.Length; i++)
				{
					bulletclone[i] = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/bullet"), transform.position, transform.rotation);
					bulletclone[i].transform.Rotate(90f, 0f, 0f);

					if (bulletSpawn != null)
                    {
                        bulletclone[i].transform.position = bulletSpawn.position;
                    } else
                    {
                        bulletSpawn = GameObject.Find(this.gameObject.name + "BulletSpawn").transform;
                    }
				}
				bulletclone[0].GetComponent<Rigidbody>().velocity = transform.parent.forward * bulletSpeed;
				bulletclone[1].GetComponent<Rigidbody>().velocity = transform.parent.forward * bulletSpeed + new Vector3(8f, 1f, 0f);
				bulletclone[2].GetComponent<Rigidbody>().velocity = transform.parent.forward * bulletSpeed + new Vector3(-8f, 1f, 0f);
					
			}
		} else if(NumberOfMagazines != 0)
        {
			isReloading = true;
		}
    }
}
