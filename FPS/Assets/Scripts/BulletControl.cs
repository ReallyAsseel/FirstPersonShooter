using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BulletControl : MonoBehaviour {
	public float bulletSpeed;
	public GameObject spawn;
	public GameObject bulletclone;
	public Vector3 offSet;
	public GameObject Muzzle;
	int i;
	public float delay, show;
	Animator anim;
    Camera camera;
    public float ADSsmoothness;
    public bool ADSOn;

	// Use this for initialization
	void Start () {
		bulletSpeed = 250.0f;
		spawn = GameObject.Find("BulletSpawn");
		offSet = new Vector3();
		Muzzle = GameObject.Find("Muzzle");
		show = 5f;
		delay = 0.1f;
		anim = GetComponentInParent<Animator>();
        camera = GameObject.FindObjectOfType<Camera>();
        ADSOn = false;
        ADSsmoothness = 3f;
        switch (gameObject.name)
        {
            case "deagle":
                bulletSpeed *= -1;
                break;
            case "SPAS 12":
                
                break;
        }
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
        if (GameObject.FindGameObjectsWithTag("Weapon").Length != 0)
        {
            ADS();
            Fire();
        }
	}

    void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Fired", true);
            i = Random.Range(1, 3);
            float a = Random.Range(0.5f, 1.5f);
            Muzzle.transform.localScale = new Vector3(a, a, a);
            Muzzle.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Muzzle" + i);
            Muzzle.GetComponent<SpriteRenderer>().enabled = true;
            bulletclone = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/bullet"), spawn.transform.position
                                                             , spawn.transform.rotation);
            bulletclone.transform.Rotate(90f, 0f, 0f);
            bulletclone.GetComponent<Rigidbody>().velocity = transform.TransformDirection(0, 0, bulletSpeed);
        }
        else
        {
            Muzzle.GetComponent<SpriteRenderer>().enabled = false;
            anim.SetBool("Fired", false);
        }
    }

    void ADS()
    {
        if (Input.GetMouseButton(1))
        {
            ADSOn = true;
            anim.SetBool("ADS", true);
            GameObject.Find("Crosshair").GetComponent<Image>().enabled = false;
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
            GameObject.Find("Crosshair").GetComponent<Image>().enabled = true;
            anim.SetBool("ADS", false);
            if (camera.fieldOfView < 59)
            {
                camera.fieldOfView += ADSsmoothness;
            }
            else if (camera.fieldOfView > 61)
            {
                camera.fieldOfView -= ADSsmoothness;
            }
            ADSOn = false;
        }
    }
}
