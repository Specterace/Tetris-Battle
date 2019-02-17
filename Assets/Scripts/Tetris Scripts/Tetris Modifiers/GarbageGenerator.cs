using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TetrisGame))]
public class GarbageGenerator : MonoBehaviour {
	private TetrisBoard board;

	int nn = 0;

	// Use this for initialization
	void Start () {
		board = gameObject.GetComponent<TetrisBoard>();
		//board.BoardChanged += Board_BoardChanged;
	}

	void Board_BoardChanged (object sender, System.EventArgs e)
	{
		if (board.Controller.Lost)
			nn = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Generates n lines of garbage
	/// </summary>
	/// <param name="n">Number of lines to generate.</param>
	public void GenerateGarbage(int n)
	{
		nn = n;
		for (int i = 0; i < nn && !board.Controller.Lost; i++)
			GenerateGarbage ();
	}

	public void GenerateGarbage()
	{
		board.Controller.ReverseCollapse (1);
		if (nn != 0) {
			int y = board.Controller.Height - 1;
			GenerateGarbageAt (y);
		}
	}

	public void GenerateGarbageAtTop()
	{
		int y = board.Controller.Height - 1;
		while (y > 0) {
			if (lineIsClear (y))
				break;
			y--;
		}
		if (y == 0)
			return;
		GenerateGarbageAt (y);
	}

	private bool lineIsClear(int line)
	{
		Tetris.Point p = new Tetris.Point (0, line);
		for (p.x = 0; p.x < board.Controller.Width; p.x++) {
			if (board.Controller [p].Occupied)
				return false;
		}
		return true;
	}

	private void GenerateGarbageAt(int line)
	{
		int x = Random.Range (0, board.Controller.Width);
		board.Controller.PlaceBlocks (
			Enumerable.Range (0, board.Controller.Width)
			.Where ((int q) => q != x)
			.Select ((int q) => new Tetris.Point (q, line)),
			Tetris.BlockColor.black);
	}
}
