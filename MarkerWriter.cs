using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.LSL4Unity.Scripts;

public class MarkerWriter : MonoBehaviour {

	private LSLMarkerStream marker;
	private int i;
	// Use this for initialization
	void Start () {
		marker = FindObjectOfType<LSLMarkerStream> ();
	}
	
	// Update is called once per frame
	void Update () {
		marker.Write ("Counting = " + i * Time.deltaTime);
		i++;
	}
}
