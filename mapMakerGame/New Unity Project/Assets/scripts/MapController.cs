using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class MapController : MonoBehaviour
{
    [HideInInspector] public Map map;

    // Start is called before the first frame update
    void Start()
    {
        map = new Map();
    }

    public void addKing(King king)
    {
        List<King> kings = map.Kings;
        kings.Add(king);
        map.Kings = kings;
    }

    public void addPath(Path path)
    {
        List<Path> paths = map.paths;
        paths.Add(path);
        map.paths = paths;
    }

    public void addRoad()
    {
        GameObject gameController = GameObject.Find("GameController");
        Instantiation instantiation = gameController.GetComponent<Instantiation>();
        GameObject road = gameController.GetComponent<MouseController>().road;
        GameObject[,] cells = instantiation.cells;
        Map map = gameController.GetComponent<MapController>().map;
        Path lastPath = map.paths[map.paths.Count - 1];
        foreach (Cell cell in lastPath.cells)
        {
            Debug.Log("cell rowfirst: " + cell.row + ", " + cell.column);
            int row = cell.row;
            int column = cell.column;
            Destroy(cells[column, row]);
            Quaternion rot = Quaternion.Euler(0, 0, 0);
            cells[column, row] = Instantiate(road, new Vector3(row + 0.5f, column + 0.5f,-0.2f), rot);
        }

        gameController.GetComponent<MouseController>().isFirstRoadCellSelected = false;
    }

    public void finishDraw()
    {
        GameObject gameController = GameObject.Find("GameController");
        Map newMap = gameController.GetComponent<MapController>().map;
        string mapJson = JsonConvert.SerializeObject(newMap);
        string destination = Application.persistentDataPath + "/map.json";
        FileStream file;
        if(File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);
        StreamWriter streamWriter = new StreamWriter(file);
        streamWriter.WriteLine(mapJson);
        streamWriter.Close();
        Debug.Log(mapJson);
    }

}


