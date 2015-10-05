using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {

    GameObject[] spawnPoints;
    float Difficulty;

    // Use this for initialization
    void Start () {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        Difficulty = 1;
    }
	
	// Update is called once per frame
	void Update () {
	    if(GameObject.FindGameObjectsWithTag("Enemy").Length <= Difficulty || GameObject.FindGameObjectsWithTag("Enemy") == null)
        {
            Difficulty++;
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
