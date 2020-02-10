using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersusDeactive : MonoBehaviour
{
    public GameObject versusPanel;
    public GameObject inGamePanel;

    public void DeActiveVersus()
    {
        versusPanel.SetActive(false);
        inGamePanel.SetActive(true);
    }
}
