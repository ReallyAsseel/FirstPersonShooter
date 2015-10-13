using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {

    GameObject[] spawnPoints;

    // Use this for initialization
    void Start () {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
    }
	
	// Update is called once per frame
	void Update () {
	    if(GameObject.FindGameObjectsWithTag("Enemy").Length <= 1 || GameObject.FindGameObjectsWithTag("Enemy") == null)
        {
            Spawn();
        }
	}

    void Spawn()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), spawnPoints[i].transform.position, Quaternion.identity);
        }
    }
}
