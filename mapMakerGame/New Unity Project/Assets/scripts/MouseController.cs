using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{

    public GameObject emptyCell;
    public GameObject active_road;
    public GameObject road;
    public GameObject tower;
    public int numOfTowers = 0;
    public Text selectPathText;
    [HideInInspector]
    public string json;
    [HideInInspector]
    public bool isFirstRoadCellSelected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject gameObject = GameObject.Find("GameController");
            Instantiation instantiation = gameObject.GetComponent<Instantiation>();
            bool isIngame = instantiation.isIngame;
            Text heightText = instantiation.HeightText;
            Text widthText = instantiation.WidthText;
            int HeightSize = int.Parse(heightText.text.ToString());
            int WidthSize = int.Parse(widthText.text.ToString());
            if (isIngame)
            {
                float cellsize;
                Vector3 mousePos = Input.mousePosition;
                if (HeightSize < WidthSize)
                {
                    cellsize = (float)Screen.height / WidthSize;
                    //mainCamera.orthographicSize = (int)(1.1 * WidthSize) / 2;
                    //scale = WidthSize / 2;
                }
                else
                {
                    cellsize = (float)Screen.height / HeightSize;

                    //mainCamera.orthographicSize = (int)(1.1 * HeightSize) / 2;
                    //scale = HeightSize / 2;
                }
                Vector3 mousePosition = Input.mousePosition;
                Vector3 cellPos = new Vector3(mousePosition.x / cellsize, mousePosition.y / cellsize , 0);
                if (cellPos.x < WidthSize && cellPos.y < HeightSize)
                {
                    cellSelected(cellPos, instantiation, new Vector2(HeightSize, WidthSize));
                }
            }
            else
            {
                
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isFirstRoadCellSelected)
            {
                Instantiation instantiation = gameObject.GetComponent<Instantiation>();
                MapController mapController = GetComponent<MapController>();
                GameObject[,] cells = instantiation.cells;
                Path lastPath = mapController.map.paths[mapController.map.paths.Count - 1];
                Cell lastPathCell = lastPath.cells[lastPath.cells.Count - 1];
                if (lastPathCell.column != 0)
                {
                    int newPathCellColumn = lastPathCell.column - 1;
                    int newPathCellRow = lastPathCell.row;
                    Cell newPathCell = new Cell(newPathCellRow, newPathCellColumn);
                    Destroy(cells[lastPathCell.column - 1, lastPathCell.row]);
                    Quaternion rot = Quaternion.Euler(0, 0, 0);
                    cells[lastPathCell.column - 1, lastPathCell.row] = Instantiate(
                        active_road, new Vector3(newPathCellRow + 0.5f, newPathCellColumn + 0.5f, -0.1f), rot);
                    mapController.map.paths[mapController.map.paths.Count - 1].cells.Add(newPathCell);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isFirstRoadCellSelected)
            {
                
                Instantiation instantiation = gameObject.GetComponent<Instantiation>();
                MapController mapController = GetComponent<MapController>();
                GameObject[,] cells = instantiation.cells;
                Path lastPath = mapController.map.paths[mapController.map.paths.Count - 1];
                Cell lastPathCell = lastPath.cells[lastPath.cells.Count - 1];
                if (lastPathCell.column != mapController.map.column - 1)
                {
                    int newPathCellColumn = lastPathCell.column + 1;
                    int newPathCellRow = lastPathCell.row;
                    Cell newPathCell = new Cell(newPathCellRow, newPathCellColumn);
                    Destroy(cells[lastPathCell.column + 1, lastPathCell.row]);
                    Quaternion rot = Quaternion.Euler(0, 0, 0);
                    cells[lastPathCell.column + 1, lastPathCell.row] = Instantiate(
                        active_road, new Vector3(newPathCellRow + 0.5f, newPathCellColumn + 0.5f, -0.1f), rot);
                    mapController.map.paths[mapController.map.paths.Count - 1].cells.Add(newPathCell);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (isFirstRoadCellSelected)
            {
                Instantiation instantiation = gameObject.GetComponent<Instantiation>();
                MapController mapController = GetComponent<MapController>();
                GameObject[,] cells = instantiation.cells;
                Path lastPath = mapController.map.paths[mapController.map.paths.Count - 1];
                Cell lastPathCell = lastPath.cells[lastPath.cells.Count - 1];
                if (lastPathCell.row != 0)
                {
                    int newPathCellColumn = lastPathCell.column;
                    int newPathCellRow = lastPathCell.row - 1;
                    Cell newPathCell = new Cell(newPathCellRow, newPathCellColumn);
                    Destroy(cells[lastPathCell.column, lastPathCell.row - 1]);
                    Quaternion rot = Quaternion.Euler(0, 0, 0);
                    cells[lastPathCell.column , lastPathCell.row - 1] = Instantiate(
                        active_road, new Vector3(newPathCellRow + 0.5f, newPathCellColumn + 0.5f, -0.1f), rot);
                    mapController.map.paths[mapController.map.paths.Count - 1].cells.Add(newPathCell);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (isFirstRoadCellSelected)
            {
                Instantiation instantiation = gameObject.GetComponent<Instantiation>();
                MapController mapController = GetComponent<MapController>();
                GameObject[,] cells = instantiation.cells;
                Path lastPath = mapController.map.paths[mapController.map.paths.Count - 1];
                Cell lastPathCell = lastPath.cells[lastPath.cells.Count - 1];
                if (lastPathCell.column != mapController.map.row - 1)
                {
                    int newPathCellColumn = lastPathCell.column;
                    int newPathCellRow = lastPathCell.row + 1;
                    Cell newPathCell = new Cell(newPathCellRow, newPathCellColumn);
                    Destroy(cells[lastPathCell.column, lastPathCell.row + 1]);
                    Quaternion rot = Quaternion.Euler(0, 0, 0);
                    cells[lastPathCell.column, lastPathCell.row + 1] = Instantiate(
                        active_road, new Vector3(newPathCellRow + 0.5f, newPathCellColumn + 0.5f, -0.1f), rot);
                    mapController.map.paths[mapController.map.paths.Count - 1].cells.Add(newPathCell);
                }
            }
        }

    }

    private void cellSelected( Vector3 cellPos, Instantiation instantiation, Vector2 Size)
    {
        int cellX = (int)cellPos.x;
        int cellY = (int)cellPos.y;
        MapController mapController = GetComponent<MapController>();
        GameObject[,] cells = instantiation.cells;
        if (numOfTowers < 4)
        {
            if (!(cellX < 1 || cellX >= Size.y - 1 || cellY < 1 || cellY >= Size.x - 1))
            {
                Quaternion rot = Quaternion.Euler(0, 0, 0);
                Destroy(cells[cellY, cellX]);
                cells[cellY, cellX] = (GameObject)Instantiate(tower, 
                    new Vector3(cellX + 0.5f, cellY + 0.5f, -0.1f), rot);
                numOfTowers++;
                mapController.addKing(new King(cellX, cellY));
            }
            if (numOfTowers == 4)
            {
                selectPathText.text = "SELECT PATHS ";
            }
            

        } else {
            if (!isFirstRoadCellSelected)
            {
                isFirstRoadCellSelected = true;
                List<Cell> activeRoadCells = new List<Cell>();
                activeRoadCells.Add(new Cell(cellX, cellY));
                Path newPath;
                // Debug.Log(mapController.map.paths.Count);
                Debug.Log(mapController.map.row);
                if (mapController.map.paths.Count != 0)
                {
                    int lastPathId = mapController.map.paths.Count;
                    newPath = new Path(activeRoadCells, lastPathId);
                }
                else {
                    newPath = new Path(activeRoadCells, 0);
                }
                mapController.addPath(newPath);
                isFirstRoadCellSelected = true;
                Destroy(cells[cellY, cellX]);
                Quaternion rot = Quaternion.Euler(0, 0, 0);
                cells[cellY, cellX] = Instantiate(active_road, new Vector3(cellX + 0.5f, cellY + 0.5f, 0), rot);
            }
        }
    }
}
