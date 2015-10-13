using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDUpdate : MonoBehaviour {
    PlayerController playerController;
    Text[] AmmoLeftHUD;
	// Use this for initialization
	void Start () {
        playerController = FindObjectOfType<PlayerController>();
        AmmoLeftHUD = GetComponentsInChildren<Text>();

	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("AmmoHUD").Length; i++)
            AmmoLeftHUD[i].text = playerController.currentWeapon.GetComponentInChildren<GunMechanics>().currentBullets + "/" + (playerController.currentWeapon.GetComponentInChildren<GunMechanics>().MagazineSize * playerController.currentWeapon.GetComponentInChildren<GunMechanics>().NumberOfMagazines);
    }
}
