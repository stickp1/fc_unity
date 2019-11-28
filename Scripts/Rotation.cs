using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

	public float x;
	public float y;
	public float z;
	public Quaternion origin;

	void Start () {
		origin = transform.rotation;
		Debug.Log (Constants._session);
		Debug.Log (Constants._pseudoSession);
		if (Constants._session != 3) {
			x = Random.Range (0.0f, 10.0f);
			y = Random.Range (0.0f, 10.0f);
			z = Random.Range (0.0f, 10.0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Constants._pseudoSession != 3)
			transform.Rotate (new Vector3 (x, y, z) * Time.deltaTime);
		else
			transform.rotation = origin;
	}
}
