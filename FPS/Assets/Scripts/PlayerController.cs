using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float forwards, right, movementSpeed, stoppingPower, jumpSpeed, horizontal, vertical,
                 maxSpeed, gravity, health;
	public bool isGrounded;
    public GameObject[] gunSlots;
    public GameObject currentWeapon;
    Camera camera;

	void Start () {
        gunSlots = new GameObject[2];
        AddWeapon("Gun", "SPAS 12", 1);
        forwards = 0f;
        right = 0f;
        jumpSpeed = 10.0f;
		movementSpeed = 0.03f;
		stoppingPower = 1.2f;
		camera = GameObject.FindObjectOfType<Camera>();
		maxSpeed = .15f;
		gravity = 0f;
	}

    void Update () {
		playerMovement();
        playerLook();
        SwitchWeapon();
        if (gunSlots[0] != null)
        {
            currentWeapon = gunSlots[0];
        }
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
    }

    void SwitchWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (gunSlots[0] != null)
            {
                gunSlots[0].transform.SetParent(FindObjectOfType<GunMechanics>().GetComponent<GunMechanics>().PrimaryGunSlot.transform);
            }

            if(gunSlots[1] != null)
            {
                gunSlots[1].transform.SetParent(FindObjectOfType<GunMechanics>().GetComponent<GunMechanics>().SecondaryGunSlot.transform);
            }
        } else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (gunSlots[0] != null)
            {
                gunSlots[0].transform.SetParent(FindObjectOfType<GunMechanics>().GetComponent<GunMechanics>().SecondaryGunSlot.transform);
            }

            if (gunSlots[1] != null)
            {
                gunSlots[1].transform.SetParent(FindObjectOfType<GunMechanics>().GetComponent<GunMechanics>().PrimaryGunSlot.transform);
            }
        }
    }

    void SwapWeapon()
    {

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void playerMovement()
    {
        GetComponent<Rigidbody>().position += transform.TransformDirection(new Vector3(right, gravity, forwards));
        GetComponent<Rigidbody>().AddForce(Physics.gravity * GetComponent<Rigidbody>().mass);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0f, jumpSpeed, 0f);
            isGrounded = false;
        } 

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (forwards < maxSpeed)
            {
                forwards += movementSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (forwards > -maxSpeed)
            {
                forwards -= movementSpeed;
            }
        }
        else
        {
            if (forwards != 0)
            {
                forwards /= stoppingPower;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (right < maxSpeed)
            {
                right += movementSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (right > -maxSpeed)
            {
                right -= movementSpeed;
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
