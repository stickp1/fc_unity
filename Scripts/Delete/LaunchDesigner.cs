using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LaunchDesigner : MonoBehaviour {

	public ToggleGroup toggles;
	public GameObject panel;

	public void onClick()
	{
		System.Diagnostics.Process process = new System.Diagnostics.Process();
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		startInfo.FileName = "cmd.exe";
		startInfo.Arguments = "/C \\meta-master\\dist\\extras-x86\\Release\\openvibe-designer.cmd --play \\4Windows\\RealDeal\\Cenários\\teste.mxs";
		process.StartInfo = startInfo;
		process.Start();
		/*
		panel.SetActive (true);
		IEnumerable<Toggle> active = toggles.ActiveToggles();
		foreach (Toggle t in active) 
			if (t.isOn)	SceneManager.LoadScene (t.tag);
		panel.SetActive (false);
		*/

	}
}
