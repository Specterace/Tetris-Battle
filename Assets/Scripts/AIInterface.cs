using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris;

public interface AIInterface: IInputManager {

    void Enter(AI ai);
    void Exit();
}
