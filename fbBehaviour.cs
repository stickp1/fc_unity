using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.LSL4Unity.Scripts;
using Assets.LSL4Unity.Scripts.Examples;
using UnityEngine.UI;

public class fbBehaviour : MonoBehaviour {

	public Transform cameraTransform;
	public LSLReceiver receiver;
	public LSLReceiverMarkers receiverMarkers;

	public GameObject pause_canvas, pause_panel, pause_text, longPause_text, getReady_text, end_text, counter_text, sliderSet, sliderBlock, sliderTrial;
	public GameObject sphere;
	public Text counter;

	private bool scaleOn = true;
	public bool smoothOn = true;
	public float smoothTime = 0.1f;
	private Vector3 velocity = Vector3.zero;

	public Vector3 speed;
	private float value;
	private float timer;


	// session 1
	public float A_1;
	public float B_1;

	// session 2
	public Vector3 velocityConst;
	Vector3 rotationFactor;
	int count;

	// session 3
	public float moveTowards = 5;
	public float offset = 0.1f;
	Vector3 startFarPoint;

	// session 4
	private bool init3;
	private bool init1;
	Vector3 startNearPoint;

	void Start(){
		startNearPoint = cameraTransform.position;
		if (Constants._session == 3)  
			initS3 ();
	}

	void initS3(){
		cameraTransform.position = new Vector3 (300f, -19.9f, 5.1f);
		cameraTransform.rotation = Quaternion.Euler (0f, -83.2f, 0f);
		startFarPoint = cameraTransform.position;
		init3 = true;
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (receiverMarkers.trial);
		if (!receiverMarkers.trial || receiverMarkers.experimentEnd) {
			pause_canvas.SetActive (true);
			foreach (Transform child in pause_canvas.transform)
				child.gameObject.SetActive (false);
			pause_panel.SetActive (true);

			if (receiverMarkers.experimentEnd)
				end_text.SetActive (true);
			else if (receiverMarkers.pause == 0)
				getReady_text.SetActive (true);
			else {
				pause_text.SetActive (true);
				counter_text.SetActive (true);
				sliderSet.SetActive (true);
				sliderBlock.SetActive (true);
				sliderTrial.SetActive (true);
				counter.text = "" + ((int)receiverMarkers.counter + 1);
			}
		} else 
			pause_canvas.SetActive (false);
		
		//Debug.Log (receiver.input);
		value = receiver.normalizedInput;
		//Debug.Log (value);
		if (value < 0f)
			value = 0f;
		else if (value > 1f)
			value = 1f;
		switch(Constants._pseudoSession){
		case 1:
			if (!init1 && Constants._session != Constants._pseudoSession) {
				cameraTransform.position = startNearPoint;
				init3 = false;
			}
			transform.Rotate (speed * Time.deltaTime);
			float x = value * A_1 + B_1;
			var targetScale = new Vector3 (x, x, x);
			Vector3 scallingSize;
			scallingSize = smoothOn ? Vector3.SmoothDamp (transform.localScale, targetScale, ref velocity, smoothTime) : targetScale;
			//Debug.Log (scallingSize);
			sphere.transform.localScale = scaleOn ? scallingSize : transform.localScale;
			break;
		case 2:
			//Debug.Log (value);
			//Debug.Log (speed * value * Time.deltaTime);
			Vector3 targetRotation = 2 * speed * value * Time.deltaTime;// + velocityConst * count * Time.deltaTime;
			rotationFactor = smoothOn ? Vector3.SmoothDamp (rotationFactor, targetRotation, ref velocity, smoothTime) : targetRotation;
			transform.Rotate (rotationFactor);
			count++;
			break;
		case 3:
			if (receiverMarkers.trial) {
				if (!init3 && Constants._session != Constants._pseudoSession)
					initS3 ();
				transform.Rotate (speed * Time.deltaTime);
				Vector3 newPos = Vector3.MoveTowards (cameraTransform.position, this.transform.position, (value - offset) * moveTowards * Time.deltaTime);
				//Debug.Log (newPos);
				cameraTransform.position = newPos;
				if (cameraTransform.position == this.transform.position)
					cameraTransform.position = startFarPoint;
			}
			//cameraTransform.Rotate (new Vector3(0, 0, value * rotateTowards * Time.deltaTime));
			break;
		case 4:
			Debug.Log ("Waiting for markers...");
			break;
		}

	}

}
