using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze1Generator : MonoBehaviour
{
    [SerializeField] PrefabDatabase prefabDB;

    [SerializeField] int mazeX = 20;
    [SerializeField] int mazeY = 20;

    [SerializeField] Transform mazeGroup;

    Maze1Cell[,] cellMap;
    Stack<Maze1Cell> pathfindingCells = new Stack<Maze1Cell>();
    bool deadEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        cellMap = new Maze1Cell[mazeX, mazeY];

        for (int x = 0; x < mazeX; x++)
        {
            for (int y = 0; y < mazeY; y++)
            {
                Maze1Cell cell = Instantiate(prefabDB.prefabs[0], mazeGroup).GetComponent<Maze1Cell>();
                cell.transform.position = new Vector3(cell.mazeSize * x, 0, cell.mazeSize * y);

                cellMap[x, y] = cell;
                cell.Init(x, y);
            }
        }

        RecursiveBackTracking(cellMap[Random.Range(0, mazeX), Random.Range(0, mazeY)]);
    }

    void RecursiveBackTracking(Maze1Cell selectedCell)
    {
        selectedCell.isVisited = true;
        List<Maze1Cell> unvisitedNeighbors = new List<Maze1Cell>();
        if (selectedCell.coordX - 1 >= 0)
        {
            Maze1Cell checkCell = cellMap[selectedCell.coordX - 1, selectedCell.coordY];
            if (!checkCell.isVisited) unvisitedNeighbors.Add(checkCell);
        }
        if (selectedCell.coordX + 1 < mazeY)
        {
            Maze1Cell checkCell = cellMap[selectedCell.coordX + 1, selectedCell.coordY];
            if (!checkCell.isVisited) unvisitedNeighbors.Add(checkCell);
        }
        if (selectedCell.coordY - 1 >= 0)
        {
            Maze1Cell checkCell = cellMap[selectedCell.coordX, selectedCell.coordY - 1];
            if (!checkCell.isVisited) unvisitedNeighbors.Add(checkCell);
        }
        if (selectedCell.coordY + 1 < mazeY)
        {
            Maze1Cell checkCell = cellMap[selectedCell.coordX, selectedCell.coordY + 1];
            if (!checkCell.isVisited) unvisitedNeighbors.Add(checkCell);
        }

        if (unvisitedNeighbors.Count > 0)
        {
            Maze1Cell nextCell = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
            if (nextCell.coordX < selectedCell.coordX)
            {
                nextCell.walls[0].SetActive(false);
                selectedCell.walls[1].SetActive(false);
            }
            else if (nextCell.coordX > selectedCell.coordX)
            {
                nextCell.walls[1].SetActive(false);
                selectedCell.walls[0].SetActive(false);
            }
            else if (nextCell.coordY < selectedCell.coordY)
            {
                nextCell.walls[3].SetActive(false);
                selectedCell.walls[2].SetActive(false);
            }
            else if (nextCell.coordY > selectedCell.coordY)
            {
                nextCell.walls[2].SetActive(false);
                selectedCell.walls[3].SetActive(false);
            }
            pathfindingCells.Push(selectedCell);
            deadEnd = false;
            if (Random.Range(0, 1f) < 0.1f) Instantiate(prefabDB.orbPrefabs[Random.Range(0, prefabDB.orbPrefabs.Length)],
                new Vector3(selectedCell.coordX * 5, 0.2f, selectedCell.coordY * 5), Quaternion.identity);
            RecursiveBackTracking(nextCell);
        }
        else if (pathfindingCells.Count > 0)
        {
            Maze1Cell previousCell = pathfindingCells.Pop();
            if (!deadEnd) Instantiate(prefabDB.enemyPrefabs[Random.Range(0, prefabDB.enemyPrefabs.Length)],
                new Vector3(selectedCell.coordX * 5, 0.2f, selectedCell.coordY * 5),Quaternion.identity);
            deadEnd = true;
            RecursiveBackTracking(previousCell);
        }
    }
}
