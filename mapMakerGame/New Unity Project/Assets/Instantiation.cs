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
    GameObject[,] cells;
    // Start is called before the first frame update
    public void makeMap()
    {
        int HeightSize = int.Parse(HeightText.text.ToString());
        int WidthSize = int.Parse(WidthText.text.ToString());
        cells = new GameObject[HeightSize, WidthSize];
        CanvasGroup getCanvasGroup = menuGameObject.GetComponent<CanvasGroup>();
        for (int i = 0; i < HeightSize; i++)
        {
            for (int j = 0; j < WidthSize; j++)
            {
                Quaternion rot = Quaternion.Euler(0, 0, 0);
                cells[i, j] = (GameObject) Instantiate(emptyCell, new Vector3(i + 0.5f , j + 0.5f, 0), rot);
          
            }
        }

        menuGameObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
