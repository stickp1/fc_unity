using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.LSL4Unity.Scripts;
using Assets.LSL4Unity.Scripts.AbstractInlets;

public class LSLReceiver : InletFloatSamples {

	public float input;
	public float normalizedInput;

	private float min;
	private float max;
	private float threshold;

	private bool pullSamplesContinuously = false;

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
	}

	protected override void Process(float[] newSample, double timeStamp)
	{
		input = newSample [0];

		min = Constants._min;
		max = Constants._max;
		threshold = Constants._thr;
		normalizedInput = (input - min) / (max - min);
		//Debug.Log (input);

	}

	public float getMin() { return min; }
}