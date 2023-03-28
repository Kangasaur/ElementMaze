using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Maze2Direction { None, Left, Right, Up, Down };
public class Maze2Cell : MonoBehaviour
{
    public Maze2Direction tunnelDirection = Maze2Direction.None;
    [HideInInspector] public bool isVisited = false;

    public float mazeSize = 5;

    public GameObject wall;
    [HideInInspector] public int coordX;
    [HideInInspector] public int coordY;

    public void Init(int x, int y)
    {
        coordX = x;
        coordY = y;
    }
}
