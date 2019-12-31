using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    [Range(1, 5)]
    public int TileSize = 1;
    public int StartY = 20;
    public float StartVelocity = 20f;
    public float NextDelay = 1.0f;
    public PathTile[] mainTiles;
    public GameObject[] junckTiles;

    private bool[,] tileLocation;
    private Queue<GameObject> mapElements = new Queue<GameObject>();
    private float deltaTileTime = 0;

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

    public void anim()
    {
        
        if (deltaTileTime <= NextDelay)
        {
            deltaTileTime += Time.deltaTime;
            return;
        }
        if (mapElements.Count == 0)
        {
            return;
        }
        deltaTileTime = 0;
        mapElements.Dequeue().GetComponent<Rigidbody>().velocity = new Vector3(0, -1 * StartVelocity, 0);
        
    }

    public void RenderMap(GameInit gameInit, string packName)
    {
        SetTileLocations(gameInit);
        PathTile tilePack = Array.Find(mainTiles, item => item.name == packName);
        ScaleTilePack(tilePack);
        if (tilePack == null)
        {
            Debug.LogError("Tile Set \' " + name + "\' not found!");
            return;
        }
        
        for (int i = 0; i < gameInit.Map.Row; ++i)
        {
            for (int j = 0; j < gameInit.Map.Col; ++j)
            {
                if(tileLocation[i, j])
                {
                    CreatePathTile(tilePack, i, j, gameInit.Map.Row, gameInit.Map.Col);

                }
                else
                {
                    float xPos = (j + 0.5f) * TileSize;
                    float zPos = (i + 0.5f) * TileSize;
                    mapElements.Enqueue(Instantiate(tilePack.junk, new Vector3(xPos, StartY, zPos), Quaternion.identity));
                }
            }
        }
        
    }

    private void SetTileLocations(GameInit gameInit)

    {
        tileLocation = new bool[gameInit.Map.Row, gameInit.Map.Col];
        foreach(InitPath path in gameInit.Map.Paths)
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

        if (neighbor == 1)
            return new TileInfo(TileType.STRAIGHT, FindStraightRotation(row, col, rowSize, colSize));

        if (neighbor == 3)
            return new TileInfo(TileType.THREEWAY, FindThreeWayRotation(row, col, rowSize, colSize));

        return FindStraightAndCornerTileInfo(row, col, rowSize, colSize);
    }

    private Quaternion FindStraightRotation(int row, int col, int rowSize, int colSize)
    {
        if (col > 0 && col < colSize - 1 && (tileLocation[row, col - 1] || tileLocation[row, col + 1]))
            return Quaternion.Euler(0, 90, 0);
            return Quaternion.identity;
    }

    private Quaternion FindThreeWayRotation(int row, int col, int rowSize, int colSize)
    {
        if (row > 0 && col < colSize - 1 && col > 0 && (row == rowSize - 1 || !tileLocation[row + 1, col]))
            return Quaternion.Euler(0, 180, 0);
        if (col < colSize - 1 && row < rowSize - 1 && row > 0 && (col == 0 || !tileLocation[row, col - 1]))
            return Quaternion.Euler(0, 90, 0);
        if (col > 0 && row < rowSize - 1 && row > 0 && (col == colSize - 1 || !tileLocation[row, col + 1]))
            return Quaternion.Euler(0, 270, 0);
        return Quaternion.identity;
    }

    private TileInfo FindStraightAndCornerTileInfo(int row, int col, int rowSize, int colSize)
    {
        if (row > 0 && row < rowSize - 1 && tileLocation[row - 1, col] && tileLocation[row + 1, col])
            return new TileInfo(TileType.STRAIGHT, Quaternion.identity);
        if (col > 0 && col < colSize - 1 && tileLocation[row, col - 1] && tileLocation[row, col + 1])
            return new TileInfo(TileType.STRAIGHT, Quaternion.Euler(0, 90, 0));

        if (col > 0 && row < rowSize - 1 && tileLocation[row + 1, col] && tileLocation[row, col - 1])
            return new TileInfo(TileType.CORNER, Quaternion.identity);
        if (col > 0 && row > 0 && tileLocation[row - 1, col] && tileLocation[row, col - 1])
            return new TileInfo(TileType.CORNER, Quaternion.Euler(0, 90, 0));
        if (col < colSize - 1 && row > 0 && tileLocation[row - 1, col] && tileLocation[row, col + 1])
            return new TileInfo(TileType.CORNER, Quaternion.Euler(0, 180, 0));
        if (col < colSize - 1 && row < rowSize - 1 && tileLocation[row - 1, col] && tileLocation[row, col - 1])
            return new TileInfo(TileType.CORNER, Quaternion.Euler(0, 180, 0));

        return new TileInfo(TileType.INVALID, Quaternion.identity);
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
                mapElements.Enqueue(Instantiate(tilePack.straightTile, new Vector3(xPos, StartY, zPos), tileInfo.rotation));
                break;
            case TileType.CORNER:
                mapElements.Enqueue(Instantiate(tilePack.cornerTile, new Vector3(xPos, StartY, zPos), tileInfo.rotation));
                break;
            case TileType.INTERSECTION:
                mapElements.Enqueue(Instantiate(tilePack.intersectionTile, new Vector3(xPos, StartY, zPos), tileInfo.rotation));
                break;
            case TileType.THREEWAY:
                mapElements.Enqueue(Instantiate(tilePack.threeWayTile, new Vector3(xPos, StartY, zPos), tileInfo.rotation));
                break;
            default:
                mapElements.Enqueue(Instantiate(tilePack.junk, new Vector3(xPos, StartY, zPos), tileInfo.rotation));
                break;
        }
    }

    private void ScaleTilePack(PathTile tilePack)
    {
        Vector3 localScale = new Vector3(TileSize / (float)tilePack.PackSize, TileSize / (float)tilePack.PackSize, TileSize / (float)tilePack.PackSize);
        tilePack.straightTile.transform.localScale = localScale;
        tilePack.cornerTile.transform.localScale = localScale;
        tilePack.intersectionTile.transform.localScale = localScale;
        tilePack.threeWayTile.transform.localScale = localScale;
        tilePack.junk.transform.localScale = localScale;
    }

    public bool IsAnimationFinished()
    {
        return (mapElements.Count == 0) && (StartY / StartVelocity <= deltaTileTime);
    }

}

[System.Serializable]
public class PathTile
{
    public string name;
    [Range(1, 10)]
    public int PackSize = 5;
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