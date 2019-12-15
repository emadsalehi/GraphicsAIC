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
                Debug.Log(cellsize);
                Debug.Log(mousePos);
                Vector3 mousePosition = Input.mousePosition;
                Debug.Log(Screen.height);
                Vector3 cellPos = new Vector3(mousePosition.x / cellsize, mousePosition.y / cellsize , 0);
                Debug.Log("mouse" + mousePosition);
                if (cellPos.x < WidthSize && cellPos.y < HeightSize)
                {
                    cellSelected(cellPos, instantiation, new Vector2(HeightSize, WidthSize));
                }
                Debug.Log(cellPos);
            }
            else
            {
                
            }
        }
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

    }

    private void cellSelected( Vector3 cellPos, Instantiation instantiation, Vector2 Size)
    {
        if (numOfTowers < 4)
        {
            int cellX = (int)cellPos.x;
            int cellY = (int)cellPos.y;
            GameObject[,] cells = instantiation.cells;
            if (!(cellX < 1 || cellX > Size.y || cellY < 1 || cellY > Size.x))
            {
                Quaternion rot = Quaternion.Euler(0, 0, 0);
                Destroy(cells[cellY, cellX]);
                cells[cellX, cellY] = (GameObject)Instantiate(tower, 
                    new Vector3(cellX + 0.5f, cellY + 0.5f, -0.1f), rot);
                numOfTowers++;
            }
            if (numOfTowers == 4)
            {
                selectPathText.text = "SELECT PATH " + 1;
            }

        } else {

        }
    }
}
