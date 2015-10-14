using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float forwards, right, movementSpeed, stoppingPower, jumpSpeed, horizontal, vertical,
                 maxSpeed, gravity, maxSprintSpeed;
    public int health;
    public bool isGrounded, isSprinting;
	public List<string> weaponNames;
	public GameObject[] gunSlots;
    public GameObject currentWeapon, secondaryWeapon;
	public Camera camera;
	public GunMechanics gunMech;
	public Crosshairs crossHairs;
	public Rigidbody rigidBody;

	void Start () {
        weaponNames = new List<string>();
        weaponNames.Add("deagle");
        weaponNames.Add("SPAS 12");
		weaponNames.Add ("G36C");
		weaponNames.Add("Druganov");
        health = 100;
        gunSlots = new GameObject[2];
		AddWeapon("Gun", "G36C");
		AddWeapon ("Gun", "SPAS 12");
		if (gunSlots[0] != null)
		{
			currentWeapon = gunSlots[0];
		}
		if (gunSlots[1] != null) {
			secondaryWeapon = gunSlots[1];
		}

		forwards = 0f;
        right = 0f;
        jumpSpeed = 15.0f;
		movementSpeed = 0.03f;
		stoppingPower = 1.2f;
		maxSpeed = .15f;
        maxSprintSpeed = .20f;
		gravity = 0f;
        isSprinting = false;
        isGrounded = false;
		rigidBody = this.GetComponent<Rigidbody> ();
		crossHairs = GameObject.Find ("CH").GetComponent<Crosshairs> ();
		while (gunMech == null) {
			gunMech = currentWeapon.GetComponentInChildren<GunMechanics> ();
		}
		camera = GameObject.Find ("Main Camera").GetComponent<Camera>();
	}

    void Update () {
		playerMovement();
        playerLook();
        SwitchWeapon();
		FireWeapon (currentWeapon);
		if(gunMech != null) {
			gunMech.Reload();
		}

		if(Input.GetKeyDown(KeyCode.R) || (Input.GetMouseButtonDown(0) && currentWeapon.GetComponentInChildren<GunMechanics>().currentBullets == 0)) {
			ReloadWeapon(currentWeapon);
		}

		if (gunMech != null) {
			crossHairs.Radius = gunMech.accuracy;
		}
		/************************************/
        if (GameObject.Find("Trash") != null) // Put this in gamemaster
        {
            GameObject.Destroy(GameObject.Find("Trash"), 3);
        }
    }

    void playerHit(GameObject enemy)
    {
        rigidBody.velocity = 5 * (transform.position - enemy.transform.position);
        health -= (int)enemy.GetComponent<EnemyMechanics>().damage;
    }

    string ProperWeaponString(string weaponpickup)
    {
        string weaponname = null;
        for (int i = 0; i < weaponNames.Count; i++)
        {
            if (weaponpickup.Contains(weaponNames[i]))
            {
                weaponname = weaponNames[i];
            }
        }
        return weaponname;
    }

    public void AddWeapon(string weaponType, string weaponName)
    {
        GameObject WEAPON = (GameObject)GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/" + ProperWeaponString(weaponName)));
        WEAPON.name = ProperWeaponString(weaponName);
		gunMech = WEAPON.GetComponentInChildren<GunMechanics>();

		if (gunSlots[0] == null) {
			WEAPON.transform.parent = GameObject.Find("GunHolder").transform;
			gunSlots.SetValue(WEAPON, 0);
            currentWeapon = gunSlots[0];
            gunMech.Start();
        } else if(gunSlots[1] == null) {
			WEAPON.transform.parent = GameObject.Find("SecondGun").transform;
			gunSlots.SetValue(WEAPON, 1);
            secondaryWeapon = gunSlots[1];
            secondaryWeapon.GetComponentInChildren<GunMechanics>().Start();
        } else 
        {
            if (WEAPON.name  != secondaryWeapon.name)
            {
                WEAPON.transform.parent = GameObject.Find("GunHolder").transform;
                WEAPON.GetComponentInChildren<GunMechanics>().bulletSpawn = GameObject.Find(WEAPON.name + "ModelBulletSpawn").transform;
                WEAPON.GetComponentInChildren<GunMechanics>().magazineSpawn = GameObject.Find(WEAPON.name + "ModelMagazineSpawn").transform;
                gunSlots.SetValue(WEAPON, 0);
                currentWeapon.transform.parent = null;
                currentWeapon.name = "Trash";
                foreach (Transform child in currentWeapon.transform)
                {
                    child.name = "Trash";
                }
                currentWeapon = gunSlots[0];
				gunMech.Start();
			}         
        }
    }

    void SwitchWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Q) && gunSlots[1] != null)
        {
			GameObject _tmp = gunSlots[0];
			gunSlots.SetValue(gunSlots[1], 0);
			gunSlots.SetValue(_tmp, 1);
            gunSlots[0].transform.SetParent(gunMech.PrimaryGunSlot.transform);
            gunSlots[1].transform.SetParent(gunMech.SecondaryGunSlot.transform);
        } 

		if (gunSlots[0] != null)
		{
			currentWeapon = gunSlots[0];
		}
		if(gunSlots[1] != null) 
		{
			secondaryWeapon = gunSlots[1];
		}
    }

    public void SwapWeapon(string weaponName)
    {
		GameObject WEAPON = (GameObject)GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/" + weaponName));
		WEAPON.name = weaponName;
		gunSlots[0] = WEAPON;
		currentWeapon = gunSlots[0];
		WEAPON.transform.parent = GameObject.Find("GunHolder").transform;
		gunSlots.SetValue(gunSlots[0], 0);
    }

	void ReloadWeapon(GameObject currentWep) {
		currentWep.GetComponentInChildren<GunMechanics>().Reload();
	}
	
	void FireWeapon(GameObject currentWep) //Includes ADS code.
	{
		if (Input.GetMouseButton(1) && !gunMech.isReloading)
		{
            if (currentWep.GetComponent<GunMovement>().isFiring && currentWep.GetComponentInChildren<GunMechanics>().CurrentRate >= currentWep.GetComponentInChildren<GunMechanics>().RateOfFire && !gunMech.isReloading)
            {
                currentWep.GetComponent<GunMovement>().Recoil();
                currentWep.GetComponentInChildren<GunMechanics>().Fire();
            }
            else
            {
                if (!gunMech.isReloading)
                {
                    currentWeapon.GetComponent<GunMovement>().ADS();
                }
                //currentWep.GetComponentInChildren<GunMechanics>().CurrentRate += 1f * Time.deltaTime;
                currentWep.GetComponentInChildren<GunMechanics>().Muzzle.GetComponent<SpriteRenderer>().enabled = false;
                currentWep.GetComponentInChildren<GunMechanics>().AimDownSights(camera);
            }
        }
        else if (currentWep.GetComponent<GunMovement>().isFiring && currentWep.GetComponentInChildren<GunMechanics>().CurrentRate >= currentWep.GetComponentInChildren<GunMechanics>().RateOfFire && !gunMech.isReloading)
        {
            currentWep.GetComponent<GunMovement>().Recoil();
            currentWep.GetComponentInChildren<GunMechanics>().Fire();
        }
        else
        {
           // currentWep.GetComponentInChildren<GunMechanics>().CurrentRate += 1f * Time.deltaTime;
            currentWep.GetComponentInChildren<GunMechanics>().Muzzle.GetComponent<SpriteRenderer>().enabled = false;
            currentWep.GetComponent<GunMovement>().Still();
            currentWep.GetComponentInChildren<GunMechanics>().ReturnFromSights(camera);
        }
    }

	void OnCollisionEnter(Collision collision) {
		if(collision.collider.tag == "Floor" && isGrounded) {
			isGrounded = false;
			gravity = -9.81f;
		} else {
			isGrounded = true;
			gravity -= 0f;
		}

        if(collision.collider.tag == "EnemyWeapon")
        {
            playerHit(collision.gameObject);
            Debug.Log(health);
        }
	}

    void playerLook()
    {
        horizontal = Input.GetAxis("Mouse X");
        vertical = Input.GetAxis("Mouse Y");
        camera.transform.Rotate(new Vector3(-vertical * 2, 0, 0));
        this.transform.Rotate(0, horizontal * 2, 0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void playerMovement()
    {
        rigidBody.position += transform.TransformDirection(new Vector3(right, gravity, forwards));
        rigidBody.AddForce(Physics.gravity * rigidBody.mass);

        if (isGrounded)
        {
            isSprinting = Input.GetKey(KeyCode.LeftShift);
        } else
        {
            isSprinting = false;
        }

        GameObject.FindGameObjectWithTag("GunHolder").GetComponent<Animator>().SetBool("Sprint", isSprinting);

        if (Input.GetKeyDown (KeyCode.Space) && isGrounded) {
			GetComponent<Rigidbody> ().velocity = new Vector3 (0f, jumpSpeed, 0f);
			isGrounded = false;
		} else if (Input.GetKey (KeyCode.Space) && !isGrounded && GetComponent<Rigidbody> ().velocity.y < 10f) {
			GetComponent<Rigidbody> ().AddForce(0, 1f, 0, ForceMode.Impulse);// = new Vector3(0f, jumpSpeed, 0f);
			//Minus jetpack fuel here
		}

        if (Input.GetKey(KeyCode.W))
        {
            if (!isSprinting)
            {
                if (forwards < maxSpeed)
                {
                    forwards += movementSpeed;
                } else if(isGrounded)
                {
                    forwards -= movementSpeed;
                }
            } else
            {
                if(forwards < maxSprintSpeed)
                {
                    forwards += movementSpeed;
                }
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (!isSprinting)
            {
                if (forwards > -maxSpeed)
                {
                    forwards -= movementSpeed;
                } else if(isGrounded)
                {
                    forwards += movementSpeed;
                }
            }
            else
            {
                if (forwards > -maxSprintSpeed)
                {
                        forwards -= movementSpeed;
                }
            }
        }
        else
        {
            if (forwards != 0)
            {
                forwards /= stoppingPower;
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (!isSprinting)
            {
                if (right < maxSpeed)
                {
                    right += movementSpeed;
                } else if(isGrounded)
                {
                    right -= movementSpeed;
                }
            }
            else
            {
                if (right < maxSprintSpeed)
                {
                        right += movementSpeed;
                }
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (!isSprinting)
            {
                if (right > -maxSpeed)
                {
                    right -= movementSpeed;
                } else if(isGrounded)
                {
                    right += movementSpeed;
                }
            } else
            {
                if(right > -maxSprintSpeed)
                {
                    right -= movementSpeed;
                }
            }
        }
        else
        {
            if (right != 0)
            {
                right /= stoppingPower;
            }
        }
    }
}
