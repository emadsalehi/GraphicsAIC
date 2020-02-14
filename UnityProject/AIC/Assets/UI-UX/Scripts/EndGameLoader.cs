using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EndGameLoader : MonoBehaviour
{
    public GameObject inGamePanel;
    public GameObject endGamePanel;
    public GameObject settingPanel;

    public void LoadEndGame()
    {
        endGamePanel.SetActive(true);
        inGamePanel.SetActive(false);
        settingPanel.SetActive(false);
    }
}
