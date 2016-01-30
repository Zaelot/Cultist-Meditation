using UnityEngine;
using System.Collections;

public class Cultist : MonoBehaviour {

	public float moveAmount = 100;
	private Vector2 position;
	// Use this for initialization
	void Start () {
		position = new Vector2 (transform.localPosition.x, transform.localPosition.y);
	}
	
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
