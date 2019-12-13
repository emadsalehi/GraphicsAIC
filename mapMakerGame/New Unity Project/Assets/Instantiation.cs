using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiation : MonoBehaviour
{
    public GameObject emptyCell;
    public int MapSize;
    GameObject[,] cells;
    // Start is called before the first frame update
    void Start()
    {
        cells = new GameObject[MapSize, MapSize];
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                Quaternion rot = Quaternion.Euler(90, 0, 0);
                cells[i, j] = (GameObject) Instantiate(emptyCell, new Vector3(i - (MapSize / 2), 0, j - (MapSize / 2)), rot);
          
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
