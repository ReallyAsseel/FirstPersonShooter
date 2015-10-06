using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float forwards, right, movementSpeed, stoppingPower, jumpSpeed, horizontal, vertical,
                 maxSpeed, gravity, health;
	public bool isGrounded;
    public List<GameObject> gunSlots;
    Camera camera;

	void Start () {
        forwards = 0f;
        right = 0f;
        jumpSpeed = 10.0f;
		movementSpeed = 0.03f;
		stoppingPower = 1.2f;
		camera = GameObject.FindObjectOfType<Camera>();
		maxSpeed = .15f;
		gravity = 0f;
        gunSlots = new List<GameObject>();
        AddWeapon("Gun", "deagle");
	}

    void Update () {
		playerMovement();
        playerLook();

    }

    void AddWeapon(string weaponType, string weaponName)
    {
        GameObject WEAPON = (GameObject)GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/" + weaponName));
        WEAPON.name = weaponName;
        WEAPON.transform.parent = GameObject.FindGameObjectWithTag("GunHolder").transform;
        WEAPON.transform.localPosition = new Vector3(0f, 0f, 0f);
        gunSlots.Add(WEAPON);
    }

    void SwitchWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {

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
