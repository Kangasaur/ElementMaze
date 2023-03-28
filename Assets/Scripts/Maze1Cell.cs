using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze1Cell : MonoBehaviour
{
    [HideInInspector] public bool isVisited = false;
    public float mazeSize = 5;

    public GameObject[] walls;

    [HideInInspector] public int coordX;
    [HideInInspector] public int coordY;


    public void Init(int x, int y)
    {
        coordX = x;
        coordY = y;
    }
}
