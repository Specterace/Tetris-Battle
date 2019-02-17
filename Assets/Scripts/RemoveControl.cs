using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveControl : MonoBehaviour {
	public TetrisGame[] games;
	public PlayerController[] players;
	public AudioSource music;

	public void Remove()
	{
		foreach (TetrisGame game in games) {
			game.InputManager.IgnoreInput = true;
		}

		foreach (PlayerController player in players) {
			player.enabled = false;
		}
		Time.timeScale = 0;
		music.volume = .4f;
	}

	public void Back()
	{
		foreach (TetrisGame game in games) {
			game.InputManager.IgnoreInput = false;
		}

		foreach (PlayerController player in players) {
			player.enabled = true;
		}
		Time.timeScale = 1;
		music.volume = 1f;
	}
}
