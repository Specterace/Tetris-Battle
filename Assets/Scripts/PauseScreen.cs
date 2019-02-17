using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour {
	public string MenuScene;
	public RemoveControl rc;

	private void Update()
	{
		if (Input.GetButtonDown ("A")) {
			SceneManager.LoadScene (MenuScene, LoadSceneMode.Single);
		} else if (Input.GetButtonDown ("B")) {
			Application.Quit ();
		} else if (Input.GetButtonDown ("Start")) {
			rc.Back ();
			gameObject.SetActive (false);
		}
	}
}
