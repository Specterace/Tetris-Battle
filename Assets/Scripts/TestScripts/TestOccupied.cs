using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOccupied : MonoBehaviour {
	public TetrisBoard board;

	void OnGUI()
	{
		if (GUI.Button (new Rect (0, 0, 500, 500), "Print stuff")) {
			string s = "";
			for (int i = 0; i < board.Height; i++) {
				for (int j = 0; j < board.Width; j++) {
					s += board.Occupied [i, j] ? '1' : '0';
				}
				s += '\n';
			}
			Debug.Log (s);
		}
	}
}
