using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    [Range(1, 5)]
    public int TileSize = 1;
    public PathTile[] mainTiles;
    public GameObject[] junckTiles;
    private bool[,] tileLocation;

    public static MapRenderer instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void RenderMap(InitMap map, string packName)
    {
        SetTileLocations(map);
        PathTile tilePack = Array.Find(mainTiles, item => item.name == packName);
        if (tilePack == null)
        {
            Debug.LogError("Tile Set \' " + name + "\' not found!");
            return;
        }
        
        for (int i = 0; i < map.Row; ++i)
        {
            for (int j = 0; j < map.Col; ++j)
            {
                if(tileLocation[i, j])
                {
                    CreatePathTile(tilePack, i, j, map.Row, map.Col);
                }
                else
                {
                    float xPos = (j + 0.5f) * TileSize;
                    float zPos = (i + 0.5f) * TileSize;
                    Instantiate(tilePack.junk, new Vector3(xPos, 0, zPos), Quaternion.identity);
                }
            }
        }
        
    }

    private void SetTileLocations(InitMap map)
    {
        tileLocation = new bool[map.Row, map.Col];
        foreach(InitPath path in map.Paths)
        {
            foreach(PathCell cell in path.Cells)
            {
                tileLocation[cell.Row, cell.Col] = true;
            }
        } 
    }

    private TileInfo FindTileInfo(int row, int col, int rowSize, int colSize)
    {
        int neighbor = NumberOfNeighbor(row, col, rowSize, colSize);
        if (neighbor == 4)
            return new TileInfo(TileType.INTERSECTION, Quaternion.identity);
        if (neighbor == 1 && row > 0 && row < rowSize - 1 && (tileLocation[row - 1, col] || tileLocation[row + 1, col]))
            return new TileInfo(TileType.STRAIGHT, Quaternion.identity);
        if (neighbor == 1 && col > 0 && col < colSize - 1 && (tileLocation[row, col - 1] || tileLocation[row, col + 1]))
            return new TileInfo(TileType.STRAIGHT, Quaternion.Euler(0, 90, 0));

        if (neighbor == 3 && row < rowSize - 1 && col < colSize - 1 && col > 0 && (row == 0 || !tileLocation[row - 1, col]))
            return new TileInfo(TileType.THREEWAY, Quaternion.identity);
        if (neighbor == 3 && row > 0 && col < colSize - 1 && col > 0 && (row == rowSize - 1 || !tileLocation[row + 1, col]))
            return new TileInfo(TileType.THREEWAY, Quaternion.Euler(0, 180, 0));
        if (neighbor == 3 && col < colSize - 1 && row < rowSize - 1 && row > 0 && (col == 0 || !tileLocation[row, col - 1]))
            return new TileInfo(TileType.THREEWAY, Quaternion.Euler(0, 90, 0));
        if (neighbor == 3 && col > 0 && row < rowSize - 1 && row > 0 && (col == colSize - 1 || !tileLocation[row, col + 1]))
            return new TileInfo(TileType.THREEWAY, Quaternion.Euler(0, 270, 0));

        if (row > 0 && row < rowSize - 1 && tileLocation[row - 1, col] && tileLocation[row + 1, col])
            return new TileInfo(TileType.STRAIGHT, Quaternion.identity);
        if (col > 0 && col < colSize - 1 && tileLocation[row, col-1] && tileLocation[row , col+1])
            return new TileInfo(TileType.STRAIGHT, Quaternion.Euler(0, 90, 0));
        if (col > 0 && row < rowSize - 1 && tileLocation[row + 1, col] && tileLocation[row, col - 1])
            return new TileInfo(TileType.CORNER, Quaternion.identity);
        if (col > 0 && row > 0 && tileLocation[row -1, col] && tileLocation[row, col - 1])
            return new TileInfo(TileType.CORNER, Quaternion.Euler(0, 90, 0));
        if (col < colSize - 1 && row > 0 && tileLocation[row - 1, col] && tileLocation[row, col + 1])
            return new TileInfo(TileType.CORNER, Quaternion.Euler(0, 180, 0));
        if (col < colSize - 1 && row < rowSize - 1 && tileLocation[row - 1, col] && tileLocation[row, col - 1])
            return new TileInfo(TileType.CORNER, Quaternion.Euler(0, 180, 0));
        
        return (new TileInfo(TileType.INVALID, Quaternion.identity));
    }

    private int NumberOfNeighbor(int row, int col, int rowSize, int colSize)
    {
        int neighbor = 0;
        if (row < rowSize - 1 && tileLocation[row + 1, col])
            neighbor++;
        if (col < colSize - 1 && tileLocation[row, col + 1])
            neighbor++;
        if (0 < row && tileLocation[row - 1, col])
            neighbor++;
        if (0 < col && tileLocation[row, col - 1])
            neighbor++;
        return neighbor;
    }

    private void CreatePathTile(PathTile tilePack, int row, int col, int rowSize, int colSize)
    {
        float xPos = (col + 0.5f) * TileSize;
        float zPos = (row + 0.5f) * TileSize;
        TileInfo tileInfo = FindTileInfo(row, col, rowSize, colSize);
        switch (tileInfo.type)
        {
            case TileType.STRAIGHT:
                Instantiate(tilePack.straightTile, new Vector3(xPos, 0, zPos), tileInfo.rotation);
                break;
            case TileType.CORNER:
                Instantiate(tilePack.cornerTile, new Vector3(xPos, 0, zPos), tileInfo.rotation);
                break;
            case TileType.INTERSECTION:
                Instantiate(tilePack.intersectionTile, new Vector3(xPos, 0, zPos), tileInfo.rotation);
                break;
            case TileType.THREEWAY:
                Instantiate(tilePack.threeWayTile, new Vector3(xPos, 0, zPos), tileInfo.rotation);
                break;
            default:
                Instantiate(tilePack.junk, new Vector3(xPos, 0, zPos), tileInfo.rotation);
                break;
        }
    }

}

[System.Serializable]
public class PathTile
{
    public string name;
    public GameObject straightTile;
    public GameObject cornerTile;
    public GameObject intersectionTile;
    public GameObject threeWayTile;
    public GameObject junk;
}

public enum TileType
{
    STRAIGHT,
    CORNER,
    INTERSECTION,
    THREEWAY,
    INVALID
}

public class TileInfo
{
    public TileType type;
    public Quaternion rotation;
    public TileInfo(TileType type, Quaternion rotation)
    {
        this.type = type;
        this.rotation = rotation;
    }
    
}