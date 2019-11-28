using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.LSL4Unity.Scripts;
using Assets.LSL4Unity.Scripts.Examples;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CalibrationText : MonoBehaviour {


	public LSLReceiverMarkers receiver_IAF;
	public LSLReceiverMarkers receiver_Range;
	public GameObject instr_panel;
	public GameObject infoS1_panel;
	public GameObject info_panel;
	public string[] instructions;
	public GameObject gameTextObject;
	public Text game_text;
	public GameObject relax_text;
	public GameObject close_text;
	public GameObject open_text;
	public GameObject complete_text;
	public GameObject howLong_text;
	public Button startSession_button;
	public Button IAF_button;
	private Image img;
	private bool open; // closed, maxMin;

	public GameObject button_panel;

	private ExampleFloatInlet inlet;
	// Use this for initialization
	void Start () {
		//neurofeedback ();
		if (Constants._session == 1) 
			infoS1_panel.SetActive (true);
		else info_panel.SetActive (true);
	}

	public void neurofeedback(){
		Constants.setRange ();
		System.Diagnostics.Process process = new System.Diagnostics.Process();
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		startInfo.FileName = "cmd.exe";
		startInfo.Arguments = "/C \\meta-master\\dist\\extras-x86\\Release\\openvibe-designer.cmd --play " + Constants.OnlineFile();//method.tag=="Hilbert" ? "NF1_calib_EO_EC.mxs" : "NF1_calib_EO_EC_nonHilbert.mxs";
		process.StartInfo = startInfo;
		process.Start();
		System.Threading.Thread.Sleep (5000);
		SceneManager.LoadScene(this.tag);
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
		//maxMin = true;
	}

	private void instruct()
	{
		System.Threading.Thread.Sleep (1000);
		complete_text.SetActive (false);
		game_text.text = instructions [Constants._session-1];
		gameTextObject.SetActive (true);
	}

	void Update() 
	{
		if (receiver_IAF.trial && !open) {
			relax_text.SetActive (false);
			close_text.SetActive (false);
			open_text.SetActive (true);
			img = instr_panel.GetComponent<Image> ();
			var tempColor = img.color;
			tempColor.a = 0f;
			img.color = tempColor;
			open = true;
		} else if (!receiver_IAF.experimentEnd && !receiver_IAF.trial && open) {
			open_text.SetActive (false);
			close_text.SetActive (true);
			img = instr_panel.GetComponent<Image> ();
			var tempColor = img.color;
			tempColor.a = 1f;
			img.color = tempColor;
			open = false;
		} else if (receiver_IAF.experimentEnd && !open) {
			open = true;
			close_text.SetActive (false);
			complete_text.SetActive (true);
			if(Constants._session == 1)
				IAF_button.interactable = true;
			else 
				startSession_button.interactable = true;
		}

		if (receiver_Range.experimentEnd) 
			startSession_button.interactable = true;
	}
		



}
