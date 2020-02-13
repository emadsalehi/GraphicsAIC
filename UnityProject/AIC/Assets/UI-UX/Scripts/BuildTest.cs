using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var logPath = PlayerPrefs.GetString("LogPath");
        GetComponent<TextMeshProUGUI>().text =  logPath;   
    }
}
