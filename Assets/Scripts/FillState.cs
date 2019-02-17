using System;
using System.Collections;
using System.Collections.Generic;
using Tetris;
using UnityEngine;

public class FillState : AIInterface {

    private AI ai;

    private int x_pos;
    Rotation rotation;
    private int prevNum;
    private int moveNum;

    List<Rotation> rotationList = new List<Rotation>() { Rotation.None, Rotation.Left, Rotation.Flip, Rotation.Right};

    private Dictionary<MinoType, Point> blockRange = new Dictionary<MinoType, Point>();

    private float CoolDown;
    [SerializeField]
    private float CoolDownTimer = 0.3f;

    public FillState(float cooldown)
    {
        CoolDownTimer = cooldown;
    }

    public void Enter(AI ai) {
        CoolDown = 0;
        this.ai = ai;

        prevNum = 0;
        moveNum = 1;

        blockRange.Add(Tetromino.L, new Point(1, 8));
        blockRange.Add(Tetromino.J, new Point(1, 8));
        blockRange.Add(Tetromino.T, new Point(1, 8));
        blockRange.Add(Tetromino.S, new Point(1, 8));
        blockRange.Add(Tetromino.Z, new Point(1, 8));
        blockRange.Add(Tetromino.O, new Point(0, 8));
        blockRange.Add(Tetromino.I, new Point(1, 7));

    }

    public Point GetRange(Rotation blockRotation, MinoType blockType) {
        if (blockRotation == Rotation.None) {
            return blockRange[blockType];
        } else if (blockRotation == Rotation.Left) {
            if (blockType == Tetromino.I) {
                return new Point(0, 9);
            } else {
                return new Point(1, 9);
            }
        } else if (blockRotation == Rotation.Right) {
            if (blockType == Tetromino.I) {
                return new Point(0, 9);
            } else {
                return new Point(0, 8);
            }
        } else {
            if (blockType == Tetromino.I) {
                return new Point(2, 8);
            } else if (blockType == Tetromino.O) {
                return new Point(1, 9);
            } else {
                return blockRange[blockType];
            }
        }
        
    }

    public void Exit() {
        
    }

    public void BestMove(MinoType block) {
        double score = -1000;
        double scoreToCheck;
        int height;
        int completeLines;
        int holes;      
        int bumpiness;
        
        for (int j = 0; j < 4; j++) {
            for (int i = GetRange(rotationList[j], block).x; i <= GetRange(rotationList[j], block).y; i++) {
                height = GetAggregateHeight(i, rotationList[j]);
                completeLines = GetCompleteLines(i, rotationList[j]);
                holes = GetHoles(i, rotationList[j]);
                bumpiness = GetBumpiness(i, rotationList[j]);

                scoreToCheck = GetScore(height, completeLines, holes, bumpiness);

                if (scoreToCheck > score && scoreToCheck != 0) {
                    score = scoreToCheck;
                    x_pos = i;
                    rotation = rotationList[j];
                }
            }
        }     
    }

    public double GetScore(int height, int completeLines, int holes, int bumpiness) {
        double a = -0.510066;
        double b = 0.760666;
        double c = -0.35663;
        double d = -0.184483;
        
        double score = (height * a) + (completeLines * b) + (holes * c) + (bumpiness * d);

        return score;
    }

    public int GetColumnHeight(bool[,] occupied, int column) {
        int total = 0;
        int potential = 0;
        for (int i = 22; i >= 2; i--) {
            potential++;
            if (occupied[i, column]) {
                total += potential;
                potential = 0;
            }
        }
        return total;
    }

    private int GetAggregateHeight(int x, Rotation rotation) {
        Point new_point = new Point(x, 0);
        bool[,] occupied = ai.tetrisGame.TestDrop(new_point, rotation);

        //string s = "";
        //for (int i = 0; i < occupied.GetLength(0); i++)
        //{
        //    for (int j = 0; j < occupied.GetLength(1); j++)
        //    {
        //        s += occupied[i, j] ? '1' : '0';
        //    }
        //    s += '\n';
        //}
        //Debug.Log(s);

        int total = 0;
        for (int i = 0; i < 10; i++) {
            total += GetColumnHeight(occupied, i);
        }

        return total;
    }

    private int GetCompleteLines(int x, Rotation rotation) {
        Point new_point = new Point(x, 0);
        bool[,] occupied = ai.tetrisGame.TestDrop(new_point, rotation);

        int total = 0;
        for (int i = 22; i >= 2; i--) {
            for (int j = 0; j < 10; j++) {
                if (!occupied[i, j]) {
                    break;
                }
                if (j == 9 && occupied[i, j]) {
                    total++;
                }
            }
        }
        return total;
    }

    private int GetHoles(int x, Rotation rotation) {
        Point new_point = new Point(x, 0);
        bool[,] occupied = ai.tetrisGame.TestDrop(new_point, rotation);

        int total = 0;
        int potential = 0;
        for (int i = 0; i < 10; i++) {
            potential = 0;
            for (int j = 22; j >= 2; j--) {
                if (occupied[j, i]) {
                    total += potential;
                    potential = 0;
                } else {
                    potential++;
                }
            }
        }
        return total;
    }

    private int GetBumpiness(int x, Rotation rotation) {
        Point new_point = new Point(x, 0);
        bool[,] occupied = ai.tetrisGame.TestDrop(new_point, rotation);

        int total = 0;
        for (int i = 0; i < 10; i+=2) {
            total += Math.Abs(GetColumnHeight(occupied, i) - GetColumnHeight(occupied, i + 1));
        }

        return total;
    }

    public TetrisAction HandleInput(float deltaTime) {
        CoolDown += deltaTime;

        ai.playerScript.Attack();

        if (CoolDown > CoolDownTimer) {
            if (moveNum != prevNum) {
                BestMove(ai.tetrisGame.game.CurrentBlock.BlockType);
            }
            
            // Debug.Log("Current Block X: " + ai.tetrisGame.game.CurrentBlock.Position.x);
            // Debug.Log("Best Position X: " + x_pos);

            CoolDown = 0;
            if (ai.tetrisGame.game.CurrentBlock.Rotation == rotation) {
                if (ai.tetrisGame.game.CurrentBlock.Position.x == x_pos) {
                    moveNum++;
                    // Debug.Log("Dropping");
                    return TetrisAction.Drop;
                } else if (ai.tetrisGame.game.CurrentBlock.Position.x > x_pos) {
                    prevNum = moveNum;
                    // Debug.Log("Moving Left");
                    return TetrisAction.Left;
                } else if (ai.tetrisGame.game.CurrentBlock.Position.x < x_pos) {
                    prevNum = moveNum;
                    // Debug.Log("Moving Right");
                    return TetrisAction.Right;
                }
            } else {
                prevNum = moveNum;
                return TetrisAction.RotateLeft;
            }
            
        }
        return TetrisAction.None;
    }
}
