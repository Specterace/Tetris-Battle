using System;
using UnityEngine;
using Tetris;

[Serializable]
public class AI : MonoBehaviour {

    public AIInterface CurrentState;

    public TetrisGame tetrisGame;
    public TetrisBoard tetrisBoard;
    public Player playerScript;

    public float Cooldown = .3f;

    private void Awake() {
        tetrisGame = GetComponent<TetrisGame>();
        tetrisBoard = GetComponent<TetrisBoard>();
        playerScript = GetComponent<Player>();
        ChangeState(new FillState(Cooldown));
    }

    public void ChangeState(AIInterface newState) {
        if (CurrentState != null) {
            CurrentState.Exit();
        }
        CurrentState = newState;
        CurrentState.Enter(this);
    }

    private TetrisAction DropBlock() {
        return TetrisAction.Drop;
    }
}
