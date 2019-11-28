using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonActions : MonoBehaviour {

	float smoothFactor = 1f;
	public Button dataButton;
	public Button confButton;
	public Button caliButton;
	public Button helpButton;
	public Button playButton;
	public GameObject Panel;
	public TMP_Dropdown method;

	int[] target= {475, 448, 595, 520, 645};

	static Vector3 targetData;
	static Vector3 targetConf;
	static Vector3 targetCali;
	static Vector3 targetHelp;
	static Vector3 targetPlay;

	GameObject Instructions;
	ToggleGroup toggles;

	static bool move, moveback; 
	static bool start = true;

	public void OpenPanel()
	{
		if (start && !move) {
			start = false;
			move = true;
			targetData = new Vector3 (-1 * target [0], 0, 0) + dataButton.transform.position;
			targetConf = new Vector3 (-1 * target [1], 0, 0) + confButton.transform.position;
			targetCali = new Vector3 (-1 * target [2], 0, 0) + caliButton.transform.position;
			targetHelp = new Vector3 (-1 * target [3], 0, 0) + helpButton.transform.position;
			targetPlay = new Vector3 (-1 * target [4], 0, 0) + playButton.transform.position;
			Panel.SetActive (true);
		}
	}

	public void ClosePanel()
	{
		move = false;
		moveback = true;
		targetData += new Vector3 (target [0], 0, 0);
		targetConf += new Vector3 (target [1], 0, 0);
		targetCali += new Vector3 (target [2], 0, 0);
		targetHelp += new Vector3 (target [3], 0, 0);
		targetPlay += new Vector3 (target [4], 0, 0);
		Panel.SetActive(false);
	}

	public void ServerStart()
	{
		Panel.SetActive (true);
		if (Panel.activeSelf){
			System.Diagnostics.Process process = new System.Diagnostics.Process ();
			System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo ();
			startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			startInfo.FileName = "cmd.exe";
			startInfo.Arguments = "/C " + Constants.OV_PATH + "openvibe-acquisition-server.cmd --config " + Constants.PATH + Constants.CONF_DIR + "\\openvibe-acquisition-server.conf";
			process.StartInfo = startInfo;
			process.Start ();
		}
	}

	public void Calibrate()
	{
		System.Diagnostics.Process process = new System.Diagnostics.Process();
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		startInfo.FileName = "cmd.exe";
		startInfo.Arguments = "/C " + Constants.OV_PATH + "openvibe-designer.cmd --play " + Constants.CalibrationFile(); 
		process.StartInfo = startInfo;
		process.Start();
		System.Threading.Thread.Sleep (5000);
		SceneManager.LoadSceneAsync (this.tag);
		
	}

	void Update()
	{
		if (move || moveback) {
			if (Mathf.Round(dataButton.transform.position[0]) == Mathf.Round(targetData[0])) 
			{
				start = true;
				moveback = false;
			}
			dataButton.transform.position = Vector3.Lerp (dataButton.transform.position, targetData, Time.deltaTime * smoothFactor);
			confButton.transform.position = Vector3.Lerp (confButton.transform.position, targetConf, Time.deltaTime * smoothFactor);
			caliButton.transform.position = Vector3.Lerp (caliButton.transform.position, targetCali, Time.deltaTime * smoothFactor);
			helpButton.transform.position = Vector3.Lerp (helpButton.transform.position, targetHelp, Time.deltaTime * smoothFactor);
			playButton.transform.position = Vector3.Lerp (playButton.transform.position, targetPlay, Time.deltaTime * smoothFactor);

		}
	}


}
