using System.Collections;
using System.Collections.Generic;

using UnityEngine;
[System.Serializable]
public class King
{
    public King(int row, int column)
    {
        this.row = row;
        this.col = column;
    }
    public int row { get; set; }
    public int col { get; set; }
}
[System.Serializable]
public class Cell
{
    public Cell(int row, int column)
    {
        this.row = row;
        this.col = column;
    }
    public int row { get; set; }
    public int col { get; set; }
}
[System.Serializable]
public class Path
{
    public Path(List<Cell> cells, int pathId)
    {
        this.cells = cells;
        pathID = pathId;
    }

    public List<Cell> cells { get; set; }
    public int pathID { get; set; }
}
[System.Serializable]
public class Map
{
    public Map()
    {
        Kings = new List<King>();
        paths = new List<Path>();
    }
    public int row { get; set; }
    public int column { get; set; }
    public List<King> Kings { get; set; }
    public List<Path> paths { get; set; }
}
[System.Serializable]
public class MapJson
{
    public int row;
    public int col;
    public List<string> Kings { get; set; }
    public List<string> paths { get; set; }
}

public class PathJson
{
    public List<string> cells { get; set; }
    public int pathID { get; set; }
}