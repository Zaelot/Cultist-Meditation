using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    // Position from Map

    private bool isAlive = true;
    private int hitPoints = 2;
    private int stuff = 0;
    private int moveAmount = 100;



	private Vector2 position;
	// Use this for initialization
	void Start () {
		position = new Vector2 (transform.localPosition.x, transform.localPosition.y);
	}


    void OnCollisionEnter(Collision collision)
    {
        // Take collider attack ability
        // Take hitpoints if enough
        if( hitPoints <= 0 )
            isAlive = false;

        //Instantiate(explosionPrefab, pos, rot);
        //Destroy(gameObject);

        //killEnemy();
        //Instantiate ( this.items )
    }

    void killEnemy() { }



    // Update is called once per frame
    void Update () {
	
   
	}

	public void MoveUp () {
		position = new Vector2 (position.x, position.y + moveAmount);
		transform.position = position;
	}

	public void MoveDown () {
		position = new Vector2 (position.x, position.y - moveAmount);
		transform.position = position;
	}

	public void MoveLeft () {
		position = new Vector2 (position.x - moveAmount, position.y);
		transform.position = position;
	}

	public void MoveRight () {
		position = new Vector2 (position.x + moveAmount, position.y);
		transform.position = position;
	}

	public void Action () {
		
	}
}
