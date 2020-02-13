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
        var path = Application.dataPath + "/Server/Log/";
        var filePaths = Directory.GetFiles(path);
        Debug.Log(filePaths[0]);
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
            go.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(paths));
        }
    }
     
    private String TakeFirstPart(String path)
    {
        var fileExtPos = path.LastIndexOf(".", StringComparison.Ordinal);
        return fileExtPos >= 0 ? path.Substring(0, fileExtPos) : null;
    }


    private void OnButtonClick(String path)
    {
        PlayerPrefs.SetString("LogPath", path);
        SceneManager.LoadScene(1);
    }
}