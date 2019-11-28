using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback : MonoBehaviour {

	//public float speed;
	public float growth = 0.0001f;
	//public float smooth = 5.0f;
	public float tiltAngle = 60.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float tiltX = Input.GetAxis ("Horizontal") * tiltAngle;
		float tiltY = Input.GetAxis ("Vertical") * tiltAngle;
		float growthX = Input.GetAxis ("Horizontal");
		transform.localScale += new Vector3 (growthX, growthX, growthX) * growth;
		transform.rotation = transform.rotation * Quaternion.Euler (tiltX, tiltY, 0);
		//transform.Rotate(new Vector3(tiltX, tiltY, 0) * Time.deltaTime);
		//transform.rotation = Quaternion.Slerp (transform.rotation, target, Time.deltaTime * smooth);
		//Vector3 move = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		//transform.position += move;
	}
}
