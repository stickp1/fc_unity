using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Inputs : MonoBehaviour {
	
	public TMP_InputField  sub_in;
	public TMP_InputField  ses_in;
	public TMP_InputField  age_in;
	public TMP_Dropdown    sex_in;
	public TMP_Dropdown    feedback;
	public TMP_Dropdown    el11;
	public TMP_Dropdown    el12;
	public TMP_Dropdown    el21;
	public TMP_Dropdown    elS22;
	public TMP_Dropdown    method;
	public GameObject 	   FC_Panel;
	public GameObject 	   el_pair;
	public TMP_InputField trialLength_in;
	public TMP_InputField pauseLength_in;
	public TMP_InputField trials_in;
	public TMP_InputField blocks_in;
	public TMP_InputField sets_in;


	public void SubjectData()
	{
		//	Create new Directories
		System.Diagnostics.Process process = new System.Diagnostics.Process();
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		startInfo.FileName = "cmd.exe";
		startInfo.Arguments = "mkdir " + Constants.PATH + "Participants\\" + sub_in.text + ";/C mkdir " + Constants.PATH + "Participants\\" + sub_in.text + "\\" + ses_in.text;
		process.StartInfo = startInfo;
		process.Start();

		// Save path to Subject & Session data
		Constants.savePath (sub_in.text, ses_in.text);

	}


	public void ExperimentData()
	{
		Constants.expConfiguration (sub_in.text, ses_in.text, age_in.text, sex_in.value,feedback.value, method.value, method.captionText.text, el11.value.ToString(), el12.value.ToString());
	}

	public void TrialData()
	{
		Constants.trialData (trialLength_in.text, pauseLength_in.text, trials_in.text, blocks_in.text, sets_in.text);
	}

	public void WhatFeedback(){
		if (feedback.value > 0) {
			el_pair.SetActive (true);
			FC_Panel.SetActive (true);
			if (feedback.value == 2)
				el_pair.SetActive (false);
		} else {
			el_pair.SetActive (false);
			FC_Panel.SetActive (false);
		}
	}



	public void AlphaMaxMin()
	{

		System.Diagnostics.Process process = new System.Diagnostics.Process();
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		startInfo.FileName = "cmd.exe";
		startInfo.Arguments = "/C " + Constants.OV_PATH + "openvibe-designer.cmd" + " --play " + Constants.RangeFile();
		process.StartInfo = startInfo;
		process.Start();
	}

	public void setRange()
	{
		Constants.setRange ();
	}


		
}
