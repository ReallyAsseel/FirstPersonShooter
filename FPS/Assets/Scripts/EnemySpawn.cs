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
        /************************************/
        if (GameObject.Find("Trash") != null) // Put this in gamemaster
        {
            GameObject.Destroy(GameObject.Find("Trash"), 3);
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
