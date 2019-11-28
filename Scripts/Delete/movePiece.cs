using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePiece : MonoBehaviour {

	public string pieceStatus = "";

	public Transform edgeParticles;

	public KeyCode placePiece;

	public bool checkPlacement;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (placePiece) && pieceStatus != "locked") {
			Vector2 mousePosition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			Vector2 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
			transform.position = objPosition;
		}

		if (Input.GetKeyDown (placePiece)) {
			checkPlacement = true;
		}
		
	}

	void OnTriggerStay2D(Collider2D other){
	
		if (checkPlacement && other.gameObject.name == gameObject.name) {
			transform.position = other.gameObject.transform.position;
			pieceStatus = "locked";
			Instantiate (edgeParticles, other.gameObject.transform.position, edgeParticles.rotation);
		}
	}
}
