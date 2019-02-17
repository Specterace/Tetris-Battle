using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {
	private void Update()
	{
		if (Input.GetButtonDown ("A")) {
			Time.timeScale = 1;
			SceneManager.LoadScene ("MainMenu", LoadSceneMode.Single);
		} else if (Input.GetButtonDown ("B")) {
			Application.Quit ();
		}
	}
}
