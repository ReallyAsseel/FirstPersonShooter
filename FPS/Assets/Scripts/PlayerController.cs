using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float forwards, right, delay, stoppingPower;
	public float horizontal, vertical;
	public float maxSpeed;
	float gravity;
	public bool isJumping;
	Camera camera;

	// Use this for initialization
	void Start () {
		delay = 0.75f;
		stoppingPower = 0.1f;
		camera = GameObject.FindObjectOfType<Camera>();
		maxSpeed = 10f;
		gravity = 0f;
	}
		// Update is called once per frame
	void Update () {
		playerMovement();

		horizontal = Input.GetAxis("Mouse X");
		vertical = Input.GetAxis("Mouse Y");
		camera.transform.Rotate(new Vector3(-vertical * 2, 0, 0));
		this.transform.Rotate(0, horizontal * 2, 0);
		Screen.lockCursor = true;

	}

	void OnCollisionEnter(Collision collision) {
		if(collision.collider.tag == "Floor" && isJumping) {
			isJumping = false;
			gravity = 0f;
		} else {
			isJumping = true;
			gravity += 9.8f;
		}
	}

	void playerMovement() {
		if(Input.GetKey(KeyCode.UpArrow)) {
			if(forwards < maxSpeed) {
				forwards += delay;
			}
		} else if(Input.GetKey(KeyCode.DownArrow)) {
			if(forwards > -maxSpeed) {
				forwards -= delay;
			}
		} else {
			forwards = 0;
		}
		
		if(Input.GetKey(KeyCode.RightArrow)) {
			if(right < maxSpeed) {
				right += delay;
			}
		} else if(Input.GetKey(KeyCode.LeftArrow)) {
			if(right > -maxSpeed) {
				right -= delay;
			}
		} else {
			right = 0;
		}
		this.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(right, 0, forwards) + Physics.gravity);

	}
}
