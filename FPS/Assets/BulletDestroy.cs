using UnityEngine;
using System.Collections;

public class BulletDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject.Destroy(this.gameObject, 5f);
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.collider.tag == "Enemy") {
			GameObject.Destroy(collision.gameObject);
		}
	}
}
