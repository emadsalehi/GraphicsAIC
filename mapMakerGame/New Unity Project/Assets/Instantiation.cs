using System.Collections;
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
    public GameObject[,] cells;
    public bool isIngame = false;
    public Text selectPathText;
    // Start is called before the first frame update
    public void makeMap()
    {
        int HeightSize = int.Parse(HeightText.text.ToString());
        int WidthSize = int.Parse(WidthText.text.ToString());
        if (HeightSize < WidthSize)
        {
            mainCamera.orthographicSize =(int)  (WidthSize) / 2;
        } else
        {
            mainCamera.orthographicSize =(int)  (HeightSize) / 2;
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
        selectPathText.gameObject.SetActive(true);
        finishButton.gameObject.SetActive(true);
        menuGameObject.gameObject.SetActive(false);
        isIngame = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
