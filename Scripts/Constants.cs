using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class Constants{

	public static int SESSION_NUMBER = 4; //DO NOT CHANGE!
	public static string DELAYED_CONF_PATH = "D:\\meta-master\\dist\\extras-x86\\Release\\share\\openvibe\\kernel\\openvibe-delayed.conf";
	public static string ACQ_CONF_PATH = "D:\\meta-master\\dist\\extras-x86\\Release\\share\\openvibe\\applications\\acquisition-server\\";

	public static string PATH = "D:\\UnityNeurofeedback\\";
	public static string SCENARIOS_DIR = "Cenários";
	public static string SUBJECTS_DIR = "Participants";
	public static string CONF_DIR = "Conf_files";
	public static string LUA_DIR = "Lua";
	public static string OV_PATH = "\\meta-master\\dist\\extras-x86\\Release\\";
	public static int ACQ_LINE_NUMBER     = 42;
	public static int DELAYED_LINE_NUMBER = 28; 

	// OpenVibe Scenarios

	// Acquisition to calculate Individual Alpha Band (both feedbacks need it)
	public static string OV_IAB = "NF0_iab.mxs";

	// Alpha
	public static string OV_RANGE_ALPHA 			= "NF1_a.mxs";
	public static string OV_CALIBRATION_ALPHA 		= "NF2_a.mxs";
	public static string OV_ONLINE_ALPHA 			= "NF3_a.mxs";

	// Functional Connectivity - Electrode pairwise
	public static string OV_RANGE_FC_PAIR 			= "NF1_pairFC_hilbert.mxs";
	public static string OV_RANGE_FC_PAIR_FFT 		= "NF1_pairFC_fft.mxs";
	public static string OV_CALIBRATION_FC_PAIR 	= "NF2_pairFC_hilbert.mxs";
	public static string OV_CALIBRATION_FC_PAIR_FFT = "NF2_pairFC_fft.mxs";
	public static string OV_ONLINE_FC_PAIR 			= "NF3_pairFC_hilbert.mxs";
	public static string OV_ONLINE_FC_PAIR_FFT 		= "NF3_pairFC_fft.mxs";

	// Functional Connectivity - Weighted Node Degree
	public static string OV_RANGE_FC_WND 			= "NF1_wndFC_hilbert.mxs";
	public static string OV_RANGE_FC_WND_FFT 		= "NF1_wndFC_ImC.mxs";
	public static string OV_CALIBRATION_FC_WND 		= "NF2_wndFC_hilbert.mxs";
	public static string OV_CALIBRATION_FC_WND_FFT	= "NF2_wndFC_ImC.mxs";
	public static string OV_ONLINE_FC_WND 			= "NF3_wndFC_hilbert.mxs";
	public static string OV_ONLINE_FC_WND_FFT 		= "NF3_wndFC_ImC.mxs";

	// Feedback standards

	public static float MIN = 0f;
	public static float MAX = 1f;
	public static float THR = 0.5f;
	public static float NORM_THR = 0.5f;
	public static float TARGET = 4f; // max size of sphere (scale) = 3.6f

	// Experiment Info

	public static int _session;// = 3;
	public static int _feedback;// = 2;
	public static int _method;// = 3;
	public static string _path;// = "D:\\UnityNeurofeedback\\Participants\\5\\3\\";
	public static float _min;//* = -1.0882f;*/= 2.9966f;
	public static float _max;//* = 2.3973f; */= 10.0657f;
	public static float _thr;//* = -1.1054f;*/ =0.665f;
	public static int _pseudoSession = _session;


	public static void saveFeedback(int value){ _feedback = value; }
	public static void saveMethod(int value){ _method = value; }
	public static void savePath(string sub, string ses){ 
		_path = PATH + SUBJECTS_DIR + "\\" + sub + "\\" + ses + "\\";
		_session = System.Int32.Parse (ses);
		_pseudoSession = _session;
		Debug.Log ("path was saved : " + _path);
	}


				// Text formatting for configuration files //

	private static string confFormat(string value){
		return "\t<SettingValue>" + value + "</SettingValue>";
	}

				// Saving configuration Files specific for the subject //

	public static void expConfiguration(string sub, string ses, string age, int sex, int feedback, int method, string method_name, string el11, string el12){
		acqServer_conf(sub, ses, age, sex);
		delayed_conf (sub, ses);
		boxes_conf (sub, ses, feedback, method, method_name, el11, el12);
		createFolders (sub, ses);
	}

	public static void trialData(string trial, string pause, string trials_nb, string blocks_nb, string sets_nb){

		// Lua script configuration
		string[] lines = File.ReadAllLines (PATH + LUA_DIR + "\\stimulations_online_raw.lua");
		string trial_duration = "\ttrial_duration = " + trial;
		string break_duration = "\tbreak_duration = " + pause;
		string trials = "\tTRIALS = " + trials_nb;
		string blocks = "\tBLOCKS = " + blocks_nb;
		string sets = "\tSETS = " + sets_nb; 
		lines [7] = trial_duration;
		lines [8] = break_duration;
		lines [9] = trials;
		lines [10] = blocks;
		lines [11] = sets;
		File.WriteAllLines (PATH + LUA_DIR + "\\stimulations_online.lua", lines);
	}

	private static void acqServer_conf(string sub, string ses, string age, int sex){
		int exp_id = (System.Int32.Parse (sub) - 1) * SESSION_NUMBER + System.Int32.Parse (ses);
		string[] lines = File.ReadAllLines(ACQ_CONF_PATH + "acquisition-server-defaults-raw.conf");
		string acq = string.Format("AcquisitionServer_Driver_BrainProductsLiveAmp_Header = ExperimentID {0} SubjectAge " + age + " SubjectGender {1} ImpedanceCheck 0 SamplingFrequency 500 Channels 32 Names ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;; Gains 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1",exp_id, sex + 1);
		lines[ACQ_LINE_NUMBER] = acq;
		File.WriteAllLines(ACQ_CONF_PATH + "acquisition-server-defaults.conf", lines);
	}

	private static void delayed_conf(string sub, string ses){
		string[] lines = File.ReadAllLines (DELAYED_CONF_PATH);
		string[] moreLines = new string[DELAYED_LINE_NUMBER + 4];
		if (lines.Length < DELAYED_LINE_NUMBER + 4) {
			for (int i = 0; i < lines.Length; i++)
				moreLines [i] = lines [i];
		} else moreLines = lines;
		moreLines [DELAYED_LINE_NUMBER	  ] = "Subject = " + sub;
		moreLines [DELAYED_LINE_NUMBER + 1] = "Session = " + ses;
		moreLines [DELAYED_LINE_NUMBER + 2] = "Subject_DirectoryName = " + SUBJECTS_DIR;
		moreLines [DELAYED_LINE_NUMBER + 3] = "Conf_DirectoryName = " + CONF_DIR;
		File.WriteAllLines (DELAYED_CONF_PATH, moreLines);
	}

	private static void boxes_conf(string sub, string ses, int feedback, int method, string method_name, string el11, string el12){
		saveFeedback (feedback);
		saveMethod (method);
		string[] lines = File.ReadAllLines(PATH + CONF_DIR + "\\Channel_Selector.txt");
		lines [1] = confFormat (el11);
		File.WriteAllLines (_path + "Channel_Selector.txt", lines);

		if (feedback > 0) {
			string[] lines3;
			switch (method) {
			case 0: // Magnitude Squared Coherence
				lines3 = File.ReadAllLines (PATH + CONF_DIR + "\\MSCoh.txt");
				break;
			case 3: // Imaginary Part of Coherency
				lines3 = File.ReadAllLines(PATH + CONF_DIR + "\\ImC.txt");
				break;
			case 6: // Phase Slope Index
				lines3 = File.ReadAllLines (PATH + CONF_DIR + "\\PSI.txt");
				break;
			default: // Hilbert Transform
				lines3 = File.ReadAllLines (PATH + CONF_DIR + "\\Hilbert.txt");
				lines3[1] = confFormat(method_name);
				break;
			}
			if (feedback == 1) {
				lines3 [2] = confFormat (el11 + "-" + el12);
				File.WriteAllLines (_path + "Connectivity_Measure.txt", lines3);
			} else if (feedback == 2){
				lines3 [2] = confFormat (":-:");
				File.WriteAllLines (_path + "Connectivity_Measure.txt", lines3);
				lines [1] = confFormat (el11);	
			}
		}
	}

	private static void createFolders(string sub, string ses){
		System.Diagnostics.Process process = new System.Diagnostics.Process ();
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo ();
		startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		startInfo.FileName = "cmd.exe";
		if (_session == 1){
			startInfo.Arguments = "/C mkdir " + _path + "IAB";
			process.StartInfo = startInfo;
			process.Start ();
		}

		startInfo.Arguments = "/C mkdir " + _path + "Calibration";  
		process.StartInfo = startInfo;
		process.Start ();


		startInfo.Arguments = "/C mkdir " + _path + "Training";  
		process.StartInfo = startInfo;
		process.Start ();
	}

				// save Range from Matlab output //

	public static void setRange()
	{
		string[] file = Directory.GetFiles (Constants._path + "\\Calibration", "*.csv");
		//Debug.Log (file [0]);
		string[] csv = File.ReadAllLines (file [0]).Skip(1).ToArray();
		List<float> valuesList = new List<float> ();
		foreach (string line in csv) {
			string[] all = line.Split (',');
			//Debug.Log (all [2]);
			valuesList.Add (float.Parse (all [2]));
		}
		float[] values = valuesList.ToArray (); 
		System.Array.Sort (values);
		_min = values[0];
		_max = values[values.Length - 1] + 0.2f * values[values.Length - 1];
		Debug.Log ("Minimum value for range is = " + _min);
		Debug.Log ("Maximum value for range is = " + _max);
	}
		
				// Return proper OV Scenario Files for each step of the session //

	public static string CalibrationFile(){
		if (_session == 1)
			return PATH + SCENARIOS_DIR + "\\" + OV_IAB;
		if (_feedback == 0)
			return PATH + SCENARIOS_DIR + "\\" + OV_CALIBRATION_ALPHA;
		switch (_method) {
		case 0:
		case 3:
		case 6:
			if (_feedback == 1)
				return PATH + SCENARIOS_DIR + "\\" + OV_CALIBRATION_FC_PAIR_FFT;
			else
				return PATH + SCENARIOS_DIR + "\\" + OV_CALIBRATION_FC_WND_FFT;
		default:
			if (_feedback == 1)
				return PATH + SCENARIOS_DIR + "\\" + OV_CALIBRATION_FC_PAIR;
			else
				return PATH + SCENARIOS_DIR + "\\" + OV_CALIBRATION_FC_WND;
			}
	}

	public static string RangeFile(){
		if (_feedback == 0)
			return PATH + SCENARIOS_DIR + "\\" + OV_RANGE_ALPHA;
		switch (_method) {
		case 0:
		case 3:
		case 6:
			if (_feedback == 1)
				return PATH + SCENARIOS_DIR + "\\" + OV_RANGE_FC_PAIR_FFT;
			else
				return PATH + SCENARIOS_DIR + "\\" + OV_RANGE_FC_WND_FFT;
		default:
			if (_feedback == 1)
				return PATH + SCENARIOS_DIR + "\\" + OV_RANGE_FC_PAIR;
			else
				return PATH + SCENARIOS_DIR + "\\" + OV_RANGE_FC_WND;
		}
	}

	public static string OnlineFile(){
		if (_feedback == 0)
			return PATH + SCENARIOS_DIR + "\\" + OV_ONLINE_ALPHA;
		else
			switch (_method) {
		case 0:	
		case 3:
		case 6:
			if (_feedback == 1)
				return PATH + SCENARIOS_DIR + "\\" + OV_ONLINE_FC_PAIR_FFT;
			else
				return PATH + SCENARIOS_DIR + "\\" + OV_ONLINE_FC_WND_FFT;
		default:
			if (_feedback == 1)
				return PATH + SCENARIOS_DIR + "\\" + OV_ONLINE_FC_PAIR;
			else
				return PATH + SCENARIOS_DIR + "\\" + OV_ONLINE_FC_WND;
			}
	}
}
