using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris;
using System;

public class TestDrop : MonoBehaviour {
    public TetrisGame game;

    bool[,] occupied = null;

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

    private int GetAggregateHeight() {
        bool[,] occupied = game.TestDrop();

        int total = 0;
        for (int i = 0; i < 10; i++) {
            total += GetColumnHeight(occupied, i);
        }

        return total;
    }

    private int GetCompleteLines() {
        bool[,] occupied = game.TestDrop();

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

    private int GetHoles() {
        bool[,] occupied = game.TestDrop();

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

    private int GetBumpiness() {
        bool[,] occupied = game.TestDrop();

        int total = 0;
        for (int i = 0; i < 10; i+=2) {
            total += Math.Abs(GetColumnHeight(occupied, i) - GetColumnHeight(occupied, i + 1));
        }

        return total;
    }

    void OnGUI()
    {
        //int Height = GetAggregateHeight();
        //int Complete = GetCompleteLines();
        //int Holes = GetHoles();
        //int Bumpiness = GetBumpiness();

        //Debug.Log(GetScore(Height, Complete, Holes, Bumpiness));
        //Debug.Log(game.game.CurrentBlock.Position);
        //if (game.game.CurrentBlock.Rotation == Rotation.Left) {
        //    Debug.Log("Left");
        //} else if (game.game.CurrentBlock.Rotation == Rotation.Right) {
        //    Debug.Log("Right");
        //}
    }
}
