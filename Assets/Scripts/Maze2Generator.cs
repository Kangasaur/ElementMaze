using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze2Generator : MonoBehaviour
{
    [SerializeField] PrefabDatabase prefabDB;
    [SerializeField] int mazeX = 59;
    [SerializeField] int mazeY = 59;
    [SerializeField] Transform mazeGroup;

    Maze2Cell[,] cellMap;
    List<Maze2Cell> unvisitedCells = new List<Maze2Cell>();
    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        cellMap = new Maze2Cell[mazeX, mazeY];
        
        for (int x = 0; x < mazeX; x++)
        {
            for (int y = 0; y < mazeY; y++)
            {
                Maze2Cell cell = Instantiate(prefabDB.prefabs[1], mazeGroup).GetComponent<Maze2Cell>();
                cell.transform.position = new Vector3(cell.mazeSize * x, 0, cell.mazeSize * y);

                cellMap[x, y] = cell;
                cell.Init(x, y);
            }
        }

        Maze2Cell startCell = cellMap[1, 1];
        unvisitedCells.Add(startCell);
        RecursiveRandomPrim(startCell);
    }

    void RecursiveRandomPrim(Maze2Cell cell)
    {
        unvisitedCells.Remove(cell);
        if (!cell.isVisited)
        {
            cell.isVisited = true;
            cell.wall.SetActive(false);
            if(cell.tunnelDirection == Maze2Direction.Right)
            {
                cellMap[cell.coordX - 1, cell.coordY].wall.SetActive(false);
            }
            else if (cell.tunnelDirection == Maze2Direction.Left)
            {
                cellMap[cell.coordX + 1, cell.coordY].wall.SetActive(false);
            }
            else if (cell.tunnelDirection == Maze2Direction.Up)
            {
                cellMap[cell.coordX, cell.coordY + 1].wall.SetActive(false);
            }
            else if (cell.tunnelDirection == Maze2Direction.Down)
            {
                cellMap[cell.coordX, cell.coordY - 1].wall.SetActive(false);
            }

            List<Maze2Cell> unvisitedNeighbors = CheckCellSurroundings(cell);

            if (unvisitedNeighbors.Count > 0)
            {
                Maze2Cell endCell = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                endCell.isVisited = true;
                endCell.wall.SetActive(false);
                if (endCell.coordX < cell.coordX)
                {
                    cellMap[cell.coordX - 1, cell.coordY].wall.SetActive(false);
                }
                else if (endCell.coordX > cell.coordX)
                {
                    cellMap[cell.coordX + 1, cell.coordY].wall.SetActive(false);
                }
                else if (endCell.coordY < cell.coordY)
                {
                    cellMap[cell.coordX, cell.coordY - 1].wall.SetActive(false);
                }
                else if (endCell.coordY > cell.coordY)
                {
                    cellMap[cell.coordX, cell.coordY + 1].wall.SetActive(false);
                }
                unvisitedNeighbors.Remove(endCell);
                unvisitedCells.AddRange(unvisitedNeighbors);
                unvisitedCells.AddRange(CheckCellSurroundings(endCell));
            }
        }

        if(unvisitedCells.Count > 0)
        {
            RecursiveRandomPrim(unvisitedCells[Random.Range(0, unvisitedCells.Count)]);
        }
    }

    List<Maze2Cell> CheckCellSurroundings(Maze2Cell cell)
    {
        List<Maze2Cell> unvisitedCells = new List<Maze2Cell>();
        if (cell.coordX - 2 > 0)
        {
            Maze2Cell checkCell = cellMap[cell.coordX - 2, cell.coordY];
            if (!checkCell.isVisited)
            {
                unvisitedCells.Add(checkCell);
                checkCell.tunnelDirection = Maze2Direction.Left;
            }
        }
        if (cell.coordX + 2 < mazeX - 1)
        {
            Maze2Cell checkCell = cellMap[cell.coordX + 2, cell.coordY];
            if (!checkCell.isVisited)
            {
                unvisitedCells.Add(checkCell);
                checkCell.tunnelDirection = Maze2Direction.Right;
            }
        }
        if (cell.coordY - 2 > 0)
        {
            Maze2Cell checkCell = cellMap[cell.coordX, cell.coordY - 2];
            if (!checkCell.isVisited)
            {
                unvisitedCells.Add(checkCell);
                checkCell.tunnelDirection = Maze2Direction.Up;
            }
        }
        if (cell.coordY + 2 < mazeY - 1)
        {
            Maze2Cell checkCell = cellMap[cell.coordX, cell.coordY + 2];
            if (!checkCell.isVisited)
            {
                unvisitedCells.Add(checkCell);
                checkCell.tunnelDirection = Maze2Direction.Down;
            }
        }

        return unvisitedCells;
    }
}
