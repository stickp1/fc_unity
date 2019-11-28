﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroy : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("Game3");
		if (objs.Length > 1)
			Destroy (this.gameObject);
		DontDestroyOnLoad (this.gameObject);
	}
	
}