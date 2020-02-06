using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DynamicScrollView : MonoBehaviour
{
    public GameObject Prefab;
    public Transform Container;
    public List<string> files = new List<string>();

  
    public void LoadLocalLogFiles()
    {
        String path = Application.dataPath + "/Server/Log/";
        String[] filePaths = Directory.GetFiles(path);
        Debug.Log(filePaths[0]);
        for (int i = 0; i < filePaths.Length; i++)
        {
            // filePaths[i] = filePaths[i].Replace('\\', '/');
            String [] pathSplitted = filePaths[i].Split('/');
            String fileNameWithODots = pathSplitted[pathSplitted.Length - 1];
            String[] fileNameWithODotsSplitted = fileNameWithODots.Split('.');
            if (fileNameWithODotsSplitted[fileNameWithODotsSplitted.Length - 1] == "meta")
            {
                continue;
            }
            GameObject go = Instantiate(Prefab);
            go.GetComponentInChildren<Text>().text = takeFirstPart(fileNameWithODots);
            go.transform.SetParent(Container);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            string paths = filePaths[i];
            go.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(paths));
        }
    }
     
    String takeFirstPart(String path)
    {
        int fileExtPos = path.LastIndexOf(".");
        if (fileExtPos >= 0)
            return path.Substring(0, fileExtPos);
        return null;
    }


    public void OnButtonClick(String path)
    {
        //string file = files[index];
        Debug.Log(path);
        PlayerPrefs.SetString("LogPath", path);
        SceneManager.LoadScene(1);
        //Debug.Log(file);
        // Process file here...
    }
}