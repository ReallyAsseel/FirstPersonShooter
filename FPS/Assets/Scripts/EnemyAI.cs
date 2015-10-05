using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	GameObject player;
	public float enemySpeed;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		enemySpeed = 8f;
    }

    // Update is called once per frame
    void Update () {
        transform.LookAt(player.transform);
        transform.position += transform.forward * enemySpeed * Time.deltaTime;
        if (Vector3.Distance(transform.position, player.transform.position) >= 5f) {
			enemySpeed = 8f;
		} else {
			enemySpeed = 0f;
		}
	}
}
