using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TetrisGame))]
public class SoundManager : MonoBehaviour {
	private TetrisBoard board;
	private TetrisGame game;

	public AudioSource Collapse;
	public AudioSource Drop;
	public AudioClip Open;
	public AudioClip Close;

	void Awake()
	{
		board = GetComponent<TetrisBoard> ();
		game = GetComponent<TetrisGame> ();
	}

	void Start()
	{
		game.BlockDropped += Game_BlockDropped;
		board.RowCollapsed += Board_RowCollapsed;
		game.BlockHeld += Game_BlockDropped;
	}

	void Board_RowCollapsed (object sender, Tetris.RowCollapseEventArgs e)
	{
		Collapse.PlayOneShot (Collapse.clip);
	}

	void Game_BlockDropped (object sender, System.EventArgs e)
	{
		Drop.PlayOneShot (Drop.clip);
	}

	void Update()
	{
		if (Input.GetButtonDown ("Start")) {
			if (Time.timeScale == 1) {
				Collapse.PlayOneShot (Open);
			}
			else{
				Collapse.PlayOneShot (Close);
			}
		}
	}
}
