using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    public bool Animation = false;
    [Range(1, 5)]
    public int TileSize = 1;
    public int StartY = 20;
    public float StartVelocity = 20f;
    public float NextDelay = 1.0f;
    public KingTower[] Towers = new KingTower[4];
    public PathTile[] mainTiles;
    public JunkTile[] junckTiles;

    private bool[,] tileLocation;
    private bool[,] kingLocation;
    //private bool[,] junkLocation;
    private Queue<KingTower> kingTowerqueue;
    private Queue<GameObject> mapElements = new Queue<GameObject>();
    private float deltaTileTime = 0;
    private System.Random random = new System.Random();

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

    void Update()
    {
        if(!Animation)
        {
            return;
        }
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
        kingTowerqueue = new Queue<KingTower>(Towers);
        SetLocations(gameInit);
        PathTile tilePack = Array.Find(mainTiles, item => item.name == packName);
        ScaleTilePack(tilePack);
        if (tilePack == null)
        {
            Debug.LogError("Tile Set \' " + name + "\' not found!");
            return;
        }
        
        for (int i = 0; i < gameInit.GraphicMap.Row; ++i)
        {
            for (int j = 0; j < gameInit.GraphicMap.Col; ++j)
            {
                if(tileLocation[i, j])
                {
                    CreatePathTile(tilePack, i, j, gameInit.GraphicMap.Row, gameInit.GraphicMap.Col);
                }

                else
                {
                    JunkTile tempTile = junckTiles[random.Next(junckTiles.Length)];
                    ScaleJunkTile(tempTile);
                    float xPos = (j) * TileSize;
                    float zPos = (i) * TileSize;
                    mapElements.Enqueue(Instantiate(tempTile.gameObject, new Vector3(xPos, StartY, zPos), Quaternion.identity));
                }
            }
        }
        for (int i = 0; i < gameInit.GraphicMap.Row; ++i)
        {
            for (int j = 0; j < gameInit.GraphicMap.Col && kingLocation[i, j]; ++j)
            {
                float xPos = (j) * TileSize;
                float zPos = (i) * TileSize;
                mapElements.Enqueue(Instantiate(ScaleKingTower(kingTowerqueue.Dequeue()).tower, new Vector3(xPos, StartY, zPos), Quaternion.identity));
            }
        }

    }

    private void SetLocations(GameInit gameInit)
    {
        tileLocation = new bool[gameInit.GraphicMap.Row, gameInit.GraphicMap.Col];
        kingLocation = new bool[gameInit.GraphicMap.Row, gameInit.GraphicMap.Col];
        //junkLocation = new bool[gameInit.GraphicMap.Row, gameInit.GraphicMap.Col];

        foreach(InitPath path in gameInit.GraphicMap.Paths)
        {
            foreach(PathCell cell in path.Cells)
            {
                tileLocation[cell.Row, cell.Col] = true;
            }
        }

        foreach (InitKing king in gameInit.GraphicMap.Kings)
        {
            kingLocation[king.Row, king.Col] = true;
        }
        /*
        for(int i = 0; i < gameInit.GraphicMap.Row; ++i)
        {
            for(int j = 0; j < gameInit.GraphicMap.Col; ++j)
            {
                junkLocation[i, j] = !(tileLocation[i, j] || kingLocation[i, j]);
            }
        }
        */
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
        float xPos = (col) * TileSize;
        float zPos = (row) * TileSize;
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

    private void ScaleJunkTile(JunkTile junk)
    {
        junk.gameObject.transform.localScale = new Vector3(TileSize / (float)junk.size, TileSize / (float)junk.size, TileSize / (float)junk.size);
    }

    private KingTower ScaleKingTower(KingTower tower)
    {
        tower.tower.transform.localScale = new Vector3(TileSize / (float)tower.size, TileSize / (float)tower.size, TileSize / (float)tower.size);
        return tower;
    }

    public bool IsAnimationFinished()
    {
        return Animation && (mapElements.Count == 0) && (StartY / StartVelocity <= deltaTileTime);
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

[System.Serializable]
public class JunkTile
{
    public GameObject gameObject;
    public int size;
}

[System.Serializable]
public class KingTower
{
    public int size;
    public GameObject tower;
}