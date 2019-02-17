using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris;

[RequireComponent(typeof(TetrisBoard))]
public class AIGame : TetrisGame {
    public AIInterface AIManager;
}
