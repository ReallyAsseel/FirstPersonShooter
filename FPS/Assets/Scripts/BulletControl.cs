using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour {
	public float bulletSpeed;
	public GameObject spawn;
	public GameObject bulletclone;
	public Vector3 offSet;
	public GameObject Muzzle;
	int i;
	public float delay, show;
	Animator anim;

	// Use this for initialization
	void Start () {
		bulletSpeed = 120.0f;
		spawn = GameObject.Find("BulletSpawn");
		offSet = new Vector3();
		Muzzle = GameObject.Find("Muzzle");
		show = 5f;
		delay = 0.1f;
		anim = GetComponentInParent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Muzzle.GetComponent<SpriteRenderer>().enabled == true) {
			if(delay > show) {
				Muzzle.GetComponent<SpriteRenderer>().enabled = false;
			} else {
				delay += 0.01f;
			}
		}

		if(Input.GetMouseButtonDown(0)) {
			anim.SetBool("Fired", true);
			i=Random.Range(1,3);
			float a = Random.Range(1,3);
			Muzzle.transform.localScale = new Vector3(a, a, a);
			Muzzle.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Muzzle" + i);
			Muzzle.GetComponent<SpriteRenderer>().enabled = true;
			bulletclone = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/bullet"), spawn.transform.position
			                                                 ,spawn.transform.rotation);
			bulletclone.transform.Rotate(90f, 0f, 0f);
			bulletclone.GetComponent<Rigidbody>().velocity = transform.TransformDirection(0,0,-bulletSpeed);
		} else {
			Muzzle.GetComponent<SpriteRenderer>().enabled = false;
			anim.SetBool("Fired", false);
		}
	}
}
