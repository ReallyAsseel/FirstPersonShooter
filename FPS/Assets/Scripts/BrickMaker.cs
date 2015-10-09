using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrickMaker : MonoBehaviour {

	GameObject[] bricks;
	// Use this for initialization
	void Start () {
		bricks = new GameObject[64];
		bricks[0] = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Brick"), new Vector3(74f, -1.6f, 79f), Quaternion.identity);

		for(int i = 1; i <= 7; i++) {
			bricks[i] = (GameObject)(GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Brick"), bricks[0].transform.position + new Vector3(bricks[0].GetComponent<Collider>().bounds.size.x * i + 0.01f,0f,0f), Quaternion.identity));
			for(int a = 1; a <= 8; a++) {
				bricks[a] = (GameObject)(GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Brick"), bricks[0].transform.position + new Vector3(bricks[0].GetComponent<Collider>().bounds.size.x * i + 0.01f,bricks[0].GetComponent<Collider>().bounds.size.y * a + 0.01f,0f), Quaternion.identity));
			}
		}


		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
