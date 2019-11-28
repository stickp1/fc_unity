using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.LSL4Unity.Scripts;
using Assets.LSL4Unity.Scripts.AbstractInlets;

public class LSLReceiverMarkersNF : InletIntSamples {

	private bool pullSamplesContinuously = false;


	private int marker;

	public bool trial;
	public bool experimentEnd = false;
	public int pause;

	private const int startTrial = 33282;
	private const int endTrial = 33283;
	private const int STOP = 32770;
	private const int OPEN_EYES = 33028;
	private const int CLOSED_EYES = 33029;
	private const int T1 = 33025;
	private const int T2 = 33026;
	private const int PAUSE = 33030;
	private const int LONG_PAUSE = 33031;

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
		Debug.Log ("Stream available!");
		pullSamplesContinuously = true;
	}

	protected override void OnStreamLost()
	{
		Debug.Log ("Stream Lost!");
		pullSamplesContinuously = false;
	}

	private void Update()
	{
		if(pullSamplesContinuously)
			pullSamples();
	}
		
	protected override void Process(int[] newSample, double timeStamp){
		marker = newSample [0];
		Debug.Log (marker);
		if (marker == T1 || marker == T2)
			trial = true;
		else if (marker == PAUSE) {
			pause = 1;
			trial = false;
		} else if (marker == LONG_PAUSE) {
			pause = 2;
			trial = false;
		} else if (marker == STOP)
			experimentEnd = true;
		Debug.Log ("Trial: " + trial);
	
	}
}
