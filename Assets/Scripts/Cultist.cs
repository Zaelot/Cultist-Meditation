using UnityEngine;
using System.Collections;

public class Cultist : MonoBehaviour {

	//public float moveAmount = 100;
	//private Vector2 position;
	private Completed.Player playerInstance;

	// Use this for initialization
	void Start () {
		playerInstance = GetComponent<Completed.Player> ();
		//position = new Vector2 (transform.localPosition.x, transform.localPosition.y);
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void MoveUp () {
//		position = new Vector2 (position.x, position.y + moveAmount);
//		transform.position = position;
		playerInstance.RelateMove( 0, 1 );
	} //End.MoveUp()

	public void MoveDown () {
//		position = new Vector2 (position.x, position.y - moveAmount);
//		transform.position = position;
		playerInstance.RelateMove( 0, -1 );
	} //End.MoveDown()

	public void MoveLeft () {
//		position = new Vector2 (position.x - moveAmount, position.y);
//		transform.position = position;
		playerInstance.RelateMove( -1, 0 );
	} //End.MoveLeft()

	public void MoveRight () {
//		position = new Vector2 (position.x + moveAmount, position.y);
//		transform.position = position;
		playerInstance.RelateMove( 1, 0 );
	} //End.MoveRight()

	public void Action () { //~Z 16.01.30 | What's this one for?
		
	} //End.Action()
}
