using System;
using UnityEngine;
using Tetris;

[Serializable]
public class ControllerInputManager : IInputManager
{
	private enum InputState
	{
		LeftDown,
		RightDown,
        Down,
		None
	}

	[Header("Repeater Parameters:")]
	public float RepeatDelay = 0.15f;
	public float RepeatRate = 0.05f;

	[Header("Controls:")]
	public string HorizontalAxis = "HorizontalP1";
	public string VerticalAxis = "VerticalP1";
	public string DropButton= "DropP1";
	public string RotateRightButton = "RotateRightP1";
	public string RotateLeftButton = "RotateLeftP1";
	public string HoldButton = "HoldP1";

	[Header("Sensitivity")]
	public float Sensitivity = .75f;
	public bool IgnoreInput = false;

	private InputState state = InputState.None;
	private float timeTillRepeat = 0;
	private bool dropDown = false;

	public TetrisAction HandleInput( float deltaTime )
	{
		if (IgnoreInput)
			return TetrisAction.None;
		
		timeTillRepeat -= deltaTime;

        if (Input.GetButtonDown(DropButton))
        {
            return TetrisAction.Drop;
        }
        else if (Input.GetButtonDown(RotateRightButton))
        {
            return TetrisAction.RotateRight;
        }
        else if (Input.GetButtonDown(RotateLeftButton))
        {
            return TetrisAction.RotateLeft;
        }
        else if (Input.GetAxisRaw(VerticalAxis) < -Sensitivity && dropDown == false)
        {
            dropDown = true;
            return TetrisAction.Drop;
        }
        else if ((Input.GetAxisRaw(HorizontalAxis) < -Sensitivity) && state == InputState.None)
        {
            state = InputState.LeftDown;
            timeTillRepeat = RepeatDelay;
            return TetrisAction.Left;
        }
        else if (Input.GetAxisRaw(HorizontalAxis) > Sensitivity && state == InputState.None)
        {
            state = InputState.RightDown;
            timeTillRepeat = RepeatDelay;
            return TetrisAction.Right;
        }
        else if (Input.GetAxisRaw(VerticalAxis) > Sensitivity && state == InputState.None)
        {
            state = InputState.Down;
            timeTillRepeat = RepeatDelay;
            return TetrisAction.Down;
        }
        else if (Input.GetButtonDown(HoldButton))
        {
            return TetrisAction.Hold;
        }
        else if (state == InputState.LeftDown)
        {
            if (!(Input.GetAxisRaw(HorizontalAxis) < -Sensitivity))
            {
                state = InputState.None;
            }
            else if (timeTillRepeat <= 0)
            {
                timeTillRepeat += RepeatRate;
                return TetrisAction.Left;
            }
        }
        else if (state == InputState.RightDown)
        {
            if (!(Input.GetAxisRaw(HorizontalAxis) > Sensitivity))
            {
                state = InputState.None;
            }
            else if (timeTillRepeat <= 0)
            {
                timeTillRepeat += RepeatRate;
                return TetrisAction.Right;
            }
        }
        else if (state == InputState.Down)
        {
            if (!(Input.GetAxisRaw(VerticalAxis) > Sensitivity))
            {
                state = InputState.None;
            }
            else if (timeTillRepeat <= 0)
            {
                timeTillRepeat += RepeatRate;
                return TetrisAction.Down;
            }
        }
        else if (dropDown && !(Input.GetAxisRaw(VerticalAxis) < -Sensitivity))
            dropDown = false;

        return TetrisAction.None;
	}
}

