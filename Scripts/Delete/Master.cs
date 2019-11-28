using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour {


	public void onClick()
	{
		System.Diagnostics.Process process = new System.Diagnostics.Process();
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		startInfo.FileName = "cmd.exe";
		startInfo.Arguments = "/C \\meta-master\\dist\\extras-x86\\Release\\openvibe-acquisition-server.cmd";
		process.StartInfo = startInfo;
		process.Start();
	}
}
