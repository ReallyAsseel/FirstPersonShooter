using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float forwards, right, delay, stoppingPower, up;
	public float horizontal, vertical;
	public float maxSpeed;
	float gravity;
	public bool isGrounded;
	Camera camera;

	// Use this for initialization
	void Start () {
        forwards = 0;
        right = 0;
        up = 0;
		delay = 0.03f;
		stoppingPower = 1.2f;
		camera = GameObject.FindObjectOfType<Camera>();
		maxSpeed = .1f;
		gravity = 0f;
	}
		// Update is called once per frame
	void Update () {
		playerMovement();

		horizontal = Input.GetAxis("Mouse X");
		vertical = Input.GetAxis("Mouse Y");
		camera.transform.Rotate(new Vector3(-vertical * 2, 0, 0));
		this.transform.Rotate(0, horizontal * 2, 0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

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

    void playerMovement()
    {
        GetComponent<Rigidbody>().position += transform.TransformDirection(new Vector3(right, gravity, forwards));
        GetComponent<Rigidbody>().AddForce(Physics.gravity * GetComponent<Rigidbody>().mass);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0f, 10f, 0f);
            isGrounded = false;
        } 

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (forwards < maxSpeed)
            {
                forwards += delay;
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (forwards > -maxSpeed)
            {
                forwards -= delay;
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
                right += delay;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (right > -maxSpeed)
            {
                right -= delay;
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
