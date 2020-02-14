using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseMapFuncs : MonoBehaviour
{

    public GameObject mainMenu, chooseMenu, runServer;
    public GameObject Prefab;
    public Transform Container;
    public List<string> files = new List<string>();

    private bool isFirstTime = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLocalMaps()
    {
        if (!isFirstTime)
        {
            isFirstTime = true;
            var path = Application.dataPath + "/Server/Map/";
            var filePaths = Directory.GetFiles(path);
            Debug.Log(filePaths.Length);
            for (int i = 0; i < filePaths.Length; i++)
            {
                // filePaths[i] = filePaths[i].Replace('\\', '/');
                var pathSplitted = filePaths[i].Split('/');
                var fileNameWithODots = pathSplitted[pathSplitted.Length - 1];
                var fileNameWithODotsSplitted = fileNameWithODots.Split('.');
                if (fileNameWithODotsSplitted[fileNameWithODotsSplitted.Length - 1] == "meta")
                {
                    continue;
                }

                var go = Instantiate(Prefab);
                go.GetComponentInChildren<Text>().text = TakeFirstPart(fileNameWithODots);
                go.transform.SetParent(Container);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                var paths = filePaths[i];
                go.GetComponent<Button>().onClick.AddListener(() => ChooseMapOnButtonClick(paths));
            }
        }
    }
    
    private String TakeFirstPart(String path)
    {
        var fileExtPos = path.LastIndexOf(".", StringComparison.Ordinal);
        return fileExtPos >= 0 ? path.Substring(0, fileExtPos) : null;
    }


    private void ChooseMapOnButtonClick(String path)
    {
        Debug.Log("called");
        chooseMenu.SetActive(!chooseMenu.activeSelf);
        Debug.Log(chooseMenu.activeSelf);
        runServer.SetActive(!chooseMenu.activeSelf);
        Debug.Log(runServer.activeSelf);
    }
}
