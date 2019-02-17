using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TetrisGame))]
public class TetrisScore : MonoBehaviour {
	private TetrisBoard board;
	private TetrisGame game;
	[SerializeField]
	private int score;

	public event scoreDelegate PlayerScored;
	public delegate void scoreDelegate(object sender, int score);

	[Header("Scores")]
	public int Single = 100;
	public int Double = 300;
	public int Triple = 500;
	public int Tetris = 800;
	public int B2BTetris = 1200;
	public int ComboPoints = 50;

	[SerializeField]
	private bool prevTetris = false;
	[SerializeField]
	private int residual = 0;
	[SerializeField]
	private int comboLevel = 0;

	void Awake() {
		board = GetComponent<TetrisBoard> ();
		game = GetComponent<TetrisGame> ();
	}

	// Use this for initialization
	void Start () {
		board.Controller.RowCollapsed += Game_BoardController_RowCollapsed;
		game.BlockDropped += Game_BlockDropped;
	}

	[SerializeField]
	private bool rowCollapsed = false;

	void Game_BlockDropped (object sender, System.EventArgs e)
	{
		if (rowCollapsed)
			comboLevel++;
		else
			comboLevel = 0;
		rowCollapsed = false;
	}

	void Game_BoardController_RowCollapsed (object sender, Tetris.RowCollapseEventArgs e)
	{
		rowCollapsed = true;
		int[] basicScores = { Single, Double, Triple, Tetris };
		int thisScore = 0;
		if (e.ClearedRows.Count > 3) {
			if (prevTetris)
				thisScore += B2BTetris;
			else
				thisScore += Tetris;
			prevTetris = true;
		} else {
			thisScore += basicScores [e.ClearedRows.Count - 1];
			prevTetris = false;
		}

		thisScore += residual;
		thisScore += comboLevel * ComboPoints;
		score += thisScore;

		int sentScore = thisScore / 100;
		residual = thisScore % 100;

		if (PlayerScored != null)
			PlayerScored (this, sentScore);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
