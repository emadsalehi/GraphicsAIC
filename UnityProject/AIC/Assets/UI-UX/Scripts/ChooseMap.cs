using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMap : MonoBehaviour
{
    public GameObject chooseMap, mainMenu, chooseMapScrollView, runServer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChooseMapOnBackPresssed()
    {
        chooseMap.SetActive(!chooseMap.activeSelf);
        mainMenu.SetActive(!chooseMap.activeSelf);
    }

    public void ChooseMapLocalLogFiles()
    {   
        runServer.SetActive(!runServer.activeSelf);
        mainMenu.SetActive(!runServer.activeSelf);
        // chooseMap.SetActive(!chooseMap.activeSelf);
        // mainMenu.SetActive(!chooseMap.activeSelf);
        // chooseMapScrollView.GetComponent<ChooseMapFuncs>().LoadLocalMaps();

    }
}
