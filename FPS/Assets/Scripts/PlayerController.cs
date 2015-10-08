using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float forwards, right, movementSpeed, stoppingPower, jumpSpeed, horizontal, vertical,
                 maxSpeed, gravity, health, maxSprintSpeed;
    public bool isGrounded, isSprinting;
    public GameObject[] gunSlots;
    public GameObject currentWeapon;
    public Camera camera;

	void Start () {
        gunSlots = new GameObject[2];
		AddWeapon("Gun", "deagle", 0);
        AddWeapon("Gun", "SPAS 12", 1);
		if (gunSlots[0] != null)
		{
			currentWeapon = gunSlots[0];
			Debug.Log(GameObject.FindGameObjectsWithTag("BulletSpawnActive")[0].name);
		}
        forwards = 0f;
        right = 0f;
        jumpSpeed = 10.0f;
		movementSpeed = 0.03f;
		stoppingPower = 1.2f;
		camera = GameObject.FindObjectOfType<Camera>();
		maxSpeed = .15f;
        maxSprintSpeed = .20f;
		gravity = 0f;
        isSprinting = false;
        isGrounded = false;
	}

    void Update () {
		playerMovement();
        playerLook();
        SwitchWeapon();
		FireWeapon (currentWeapon);
		if(currentWeapon.GetComponentInChildren<GunMechanics>().isReloading) {
			currentWeapon.GetComponentInChildren<GunMechanics>().Reload();
		}
        GameObject.Find("CH").GetComponent<Crosshairs>().Radius = currentWeapon.GetComponentInChildren<GunMechanics>().accuracy;
    }

    void AddWeapon(string weaponType, string weaponName, int slot)
    {
        GameObject WEAPON = (GameObject)GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/" + weaponName));
        WEAPON.name = weaponName;
        if (slot == 0)
        {
            WEAPON.transform.parent = GameObject.Find("GunHolder").transform;
            gunSlots.SetValue(WEAPON, slot);
        } else if(slot == 1)
        {
            WEAPON.transform.parent = GameObject.Find("SecondGun").transform;
            gunSlots.SetValue(WEAPON, slot);
        }
		if (gunSlots[0] != null)
		{
			currentWeapon = gunSlots[0];
			currentWeapon.GetComponentInChildren<GunMechanics>().Start();
		}
    }

    void SwitchWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Q) && gunSlots[1] != null)
        {
			GameObject _tmp = gunSlots[0];
			gunSlots.SetValue(gunSlots[1], 0);
			gunSlots.SetValue(_tmp, 1);
            gunSlots[0].transform.SetParent(FindObjectOfType<GunMechanics>().GetComponent<GunMechanics>().PrimaryGunSlot.transform);
            gunSlots[1].transform.SetParent(FindObjectOfType<GunMechanics>().GetComponent<GunMechanics>().SecondaryGunSlot.transform);
        } 

		if (gunSlots[0] != null)
		{
			currentWeapon = gunSlots[0];
		}
    }

    void SwapWeapon()
    {

    }

	void FireWeapon(GameObject currentWep) //Includes ADS code.
	{
        if (Input.GetMouseButton(1))
        {
            if (currentWep.GetComponent<GunMovement>().isFiring && currentWep.GetComponentInChildren<GunMechanics>().CurrentRate >= currentWep.GetComponentInChildren<GunMechanics>().RateOfFire && !currentWeapon.GetComponentInChildren<GunMechanics>().isReloading)
            {
                currentWeapon.GetComponent<GunMovement>().Recoil();
                currentWep.GetComponentInChildren<GunMechanics>().Fire();
            }
            else
            {
                currentWeapon.GetComponent<GunMovement>().ADS();
                currentWep.GetComponentInChildren<GunMechanics>().CurrentRate += 1f * Time.deltaTime;
                currentWep.GetComponentInChildren<GunMechanics>().Muzzle.GetComponent<SpriteRenderer>().enabled = false;
                currentWep.GetComponentInChildren<GunMechanics>().AimDownSights(camera);
            }
        }
        else if (currentWep.GetComponent<GunMovement>().isFiring && currentWep.GetComponentInChildren<GunMechanics>().CurrentRate >= currentWep.GetComponentInChildren<GunMechanics>().RateOfFire && !currentWeapon.GetComponentInChildren<GunMechanics>().isReloading)
        {
            currentWeapon.GetComponent<GunMovement>().Recoil();
            currentWep.GetComponentInChildren<GunMechanics>().Fire();
        }
        else
        {
            currentWep.GetComponentInChildren<GunMechanics>().CurrentRate += 1f * Time.deltaTime;
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
	}

    void playerLook()
    {
        horizontal = Input.GetAxis("Mouse X");
        vertical = Input.GetAxis("Mouse Y");
        camera.transform.Rotate(new Vector3(-vertical * 2, 0, 0));
        this.transform.Rotate(0, horizontal * 2, 0);
       // Cursor.lockState = CursorLockMode.Locked;
       // Cursor.visible = false;
    }

    void playerMovement()
    {
        GetComponent<Rigidbody>().position += transform.TransformDirection(new Vector3(right, gravity, forwards));
        GetComponent<Rigidbody>().AddForce(Physics.gravity * GetComponent<Rigidbody>().mass);

        if (isGrounded)
        {
            isSprinting = Input.GetKey(KeyCode.LeftShift);
            GameObject.FindGameObjectWithTag("GunHolder").GetComponent<Animator>().SetBool("Sprint", isSprinting);
        } else
        {
            isSprinting = false;
        }

        

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0f, jumpSpeed, 0f);
            isGrounded = false;
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
