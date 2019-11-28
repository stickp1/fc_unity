using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.LSL4Unity.Scripts;
using Assets.LSL4Unity.Scripts.AbstractInlets;
using UnityEngine.UI;

public class LSLReceiverMarkers : InletIntSamples {

	private bool pullSamplesContinuously = false;


	private int marker;

	public bool trial;
	public bool experimentEnd = false;
	public int pause;
	public float counter;

	//private const int startTrial = 33282;
	//private const int endTrial = 33283;
	private const int STOP = 32770;
	private const int OPEN_EYES = 33036;// 33028;
	private const int CLOSED_EYES = 33037;//33029;
	private enum TRIALS {S1B1T1=1, S1B1T2, S1B2T1, S1B2T2, S1B3T1, S1B3T2, S2B1T1, S2B1T2, S2B2T1, S2B2T2, S2B3T1, S2B3T2, S3B1T1, S3B1T2, S3B2T1, S3B2T2, S3B3T1, S3B3T2, S4B1T1, S4B1T2, S4B2T1, S4B2T2, S4B3T1, S4B3T2, S5B1T1, S5B1T2, S5B2T1, S5B2T2, S5B3T1, S5B3T2};
	private const int PAUSE = 50;
	private const int LONG_PAUSE = 51;
	private const int LONG_PAUSE_EXTRA = 52;

	public Slider sliderSet;
	public Slider sliderBlock;
	public Slider sliderTrial;


	/*
	void Start()
	{
		// [optional] call this only, if your gameobject hosting this component
		// got instantiated during runtime

		 //registerAndLookUpStream();
	}
	/*
	protected override void AdditionalStart()
	{
		trial = false;
	}
	*/

	protected override bool isTheExpected(LSLStreamInfoWrapper stream)
	{
		// the base implementation just checks for stream name and type
		var predicate = base.isTheExpected(stream);
		// add a more specific description for your stream here specifying hostname etc.
		//predicate &= stream.HostName.Equals("Expected Hostname");
		return predicate;
	}

	protected override void OnStreamAvailable()
	{
		pullSamplesContinuously = true;
	}

	protected override void OnStreamLost()
	{
		pullSamplesContinuously = false;
	}

	private void Update()
	{
		if(pullSamplesContinuously)
			pullSamples();
		counter -= Time.deltaTime;
	}
		
	/*
	protected override void Process(int[] newSample, double timeStamp)
	{
		marker = newSample [0];

		if (marker == startTrial)
			trial = true;
		else if (marker == endTrial)
			trial = false;
		else if (marker == experimentStop)
			experimentEnd = true;
		Debug.Log ("Trial: " + trial);
	}
	*/

	protected override void Process(int[] newSample, double timeStamp){
		
		marker = newSample [0];
		Debug.Log (marker);

		if (System.Enum.IsDefined (typeof(TRIALS), marker)) {
			float aux = (marker + marker % 2)/2f;
			sliderSet.value = ((aux-1) / 3) / 5f + 0.2f;
			sliderBlock.value = aux % 3 == 0 ? 1 : (aux % 3)/3f; // resto 1 -> resto 2 -> resto 0 ;'(
			sliderTrial.value = (2 - marker % 2)/2f;
		}
		if (marker == OPEN_EYES || System.Enum.IsDefined (typeof(TRIALS), marker)) {
			trial = true;
			if (Constants._session == 4)
				switch (marker) {
				case (int)TRIALS.S1B1T1:
				case (int)TRIALS.S4B1T1:
					Constants._pseudoSession = 1;
					break;
				case (int)TRIALS.S2B1T1:
					Constants._pseudoSession = 2;
					break;
				case (int)TRIALS.S3B1T1:
				case (int)TRIALS.S5B1T1:
					Constants._pseudoSession = 3;
					break;
				}
		}
		else if (marker == CLOSED_EYES)
			trial = false;
		else if (marker == PAUSE) {
			pause = 1;
			trial = false;
			counter = 10;
		} else if (marker == LONG_PAUSE || marker == LONG_PAUSE_EXTRA) {
			pause = 2;
			trial = false;
			counter = 15;
		} else if (marker == STOP)
			experimentEnd = true;
		Debug.Log ("Trial: " + trial);
	}

		
}
