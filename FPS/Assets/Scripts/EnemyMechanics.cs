using UnityEngine;
using System.Collections;

public class EnemyMechanics : MonoBehaviour {
    public float health, speed, damage;
    public string enemyType;
    public bool readyToAttack, canMove;
    public float timer, enemyROF, wanderTimer, timeBetweenMoves, rotationDampening;
    public Vector3 randomPosition;
	public GameObject player;
    public enum States
    {
        Chase,
        Patrol
    }
    States currentState;

    // Use this for initialization
    void Start () {
		rotationDampening = 1.5f;
		player = GameObject.Find("Player");
        randomPosition = new Vector3((int)Random.Range(transform.position.x - 10, transform.position.x + 15), 0f, (int)Random.Range(transform.position.z - 15, transform.position.z + 10));
        enemyROF = 1f;
        wanderTimer = 0f;
        timer = 0f;
        canMove = false;
        readyToAttack = false;
        enemyType = "Basic";
        switch (enemyType)
        {
            case "Basic":
                health = 100f;
                speed = 8f;
                damage = 10f;
                break;

        }
        currentState = States.Patrol;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (transform.position.y < -100) {
			Destroy(gameObject);
		}

		if(CheckForPlayer(player))
		{
			currentState = States.Chase;
		} else {
			currentState = States.Patrol;
		}

        switch(currentState)
        {
            case States.Patrol:
                Wander();

                break;
            case States.Chase:
            	TowardsPlayer(GameObject.Find("Player"));
				//LookAtPlayer(player);
                Attack(readyToAttack);
                break;
        }
        
	}

    void Wander()
    {
        Debug.DrawLine(transform.position, randomPosition);
        if (Vector3.Distance(randomPosition, transform.position) > 4f) // Player distance
        {
            speed = 8f; //Enemy Speed. If the enemy has not reached its random destination for patrol, it will keep going
            transform.position += new Vector3(-transform.position.x + randomPosition.x, 0f, -transform.position.z + randomPosition.z).normalized * speed * Time.deltaTime;
        } else
        {
            speed = 0f; //If the enemy has reached its random patrol destination it will stop
        }
        if(wanderTimer <= Random.Range(1f, 5f))
        {
            wanderTimer += Time.deltaTime;
        } else
        {
            randomPosition = new Vector3((int)Random.Range(transform.position.x - 10, transform.position.x + 15), 00f, (int)Random.Range(transform.position.z - 15, transform.position.z + 10 ));
            wanderTimer = 0f; //Finally it will find another random position
        }
    }
    
    bool CheckForPlayer(GameObject player)
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= (25f + (10f/player.GetComponent<PlayerController>().health)*20)) //Check if enemy sees the player in sight
        {
            return true; 
        } else
        {
            return false;
        }
    }

	void LookAtPlayer(GameObject player) {
		Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDampening * rotationDampening);
	}

    void TowardsPlayer(GameObject player)
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, player.transform.position) >= 2.5f && timer == 0)
        {
            readyToAttack = false;
            speed = 8f;
            GetComponentInChildren<Animator>().SetBool("Attack", false);
        }
        else
        {
            speed = 0f;
            readyToAttack = true;
	        
		}
    }

    void Attack(bool isReady)
    {
        if(isReady)
        {
            if(timer < enemyROF)
            {
                timer += 1 * Time.deltaTime;
                this.gameObject.GetComponentsInChildren<Transform>()[1].eulerAngles = new Vector3(0f, 0f, 270f);
            }
            else
            {
                //Make an attack animation here
                GetComponentInChildren<Animator>().SetBool("Attack", true);
                timer = 0f;
            }
        }
    }
}
