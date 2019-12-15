﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Instantiation : MonoBehaviour
{
    public GameObject emptyCell;
    public Text HeightText;
    public Text WidthText;
    public GameObject menuGameObject;
    public Button finishButton; 
    public Camera mainCamera;
    GameObject[,] cells;
    // Start is called before the first frame update
    public void makeMap()
    {
        int HeightSize = int.Parse(HeightText.text.ToString());
        int WidthSize = int.Parse(WidthText.text.ToString());
        int scale = 0;
        if (HeightSize < WidthSize)
        {
            mainCamera.orthographicSize =(int)  (1.1 * WidthSize) / 2;
            scale = WidthSize / 2;
        } else
        {
            mainCamera.orthographicSize =(int) (1.1 * HeightSize) / 2;
            scale = HeightSize / 2;
        }
        Vector3 cameraLocation = mainCamera.transform.position;
        float aspectRatio = (float)Screen.width / Screen.height;
        mainCamera.transform.position += new Vector3(cameraLocation.x + mainCamera.orthographicSize * aspectRatio ,
            cameraLocation.y + mainCamera.orthographicSize, cameraLocation.z);
        cells = new GameObject[HeightSize, WidthSize];
        CanvasGroup getCanvasGroup = menuGameObject.GetComponent<CanvasGroup>();
        for (int i = 0; i < HeightSize; i++)
        {
            for (int j = 0; j < WidthSize; j++)
            {
                Quaternion rot = Quaternion.Euler(0, 0, 0);
                cells[i, j] = (GameObject) Instantiate(emptyCell, new Vector3(j + 0.5f , i + 0.5f, 0), rot);
          
            }
        }
        finishButton.gameObject.SetActive(true);
        menuGameObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
