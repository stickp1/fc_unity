using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LaunchGame : MonoBehaviour {

	public Text loading;

	public void onClick()
	{
		loading.gameObject.SetActive(true);
		StartCoroutine (LoadGame ());
	}

	IEnumerator LoadGame()
	{
		AsyncOperation game = SceneManager.LoadSceneAsync (this.tag);

		while (!game.isDone) 
		{
			loading.text = "Loading " + game.progress.ToString("p");
			yield return null;
		}
	}
}
