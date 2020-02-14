using System.Collections;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
// using SFB;

public class playMenuFunctions : MonoBehaviour
{
    public GameObject mainMenu, runServer;
    [FormerlySerializedAs("runSeverMenu")] public GameObject chooseMapMenu;
    public List<InputField> clientSelectFields;

    [HideInInspector]
    bool[] isSelected = new bool[4];

    public void OnClickPlay()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        chooseMapMenu.SetActive(!mainMenu.activeSelf);
        UnityEngine.Debug.Log(Application.dataPath);
    }

    public void PlayOnBackButtonPressed()
    {
        // int i = 1;
        // foreach(InputField input in clientSelectFields)
        // {
        //     input.text = "Select Client " + i + ":";
        //     i++;
        // }
        runServer.SetActive(!runServer.activeSelf);
        mainMenu.SetActive(!runServer.activeSelf);
    }


    public void SelectClientPath(int i)
    {
        // string panelName = "Load Client " + i;
        // var extensions = new []
        // {
        //     new ExtensionFilter("Client Files", "cpp", "java", "py", "go"),
        // };
        // string[] pathArray = StandaloneFileBrowser.OpenFilePanel(panelName, "", extensions, false);
        // isSelected[i] = true;
        // string path = pathArray[0];
        // InputField selectedInputField = clientSelectFields[i];
        // selectedInputField.text = path;
    }

    public void StartGame()
    {
        // string[] pathArray = StandaloneFileBrowser.OpenFolderPanel("Hello", "", false);
        // string path = pathArray[0];
        // UnityEngine.Debug.Log(path);
        // for (int i = 0; i < 4; i ++)
        // {
        //     RunClientCode(i);
        // }
        // path = path.Replace("\"", "\"\"");
        // Process.Start("/bin/bash", "-c "+"\"touch " + path+"/hello\"");
    }

    private void RunClientCode(int i)
    {
        string[] codeExtensionArray = clientSelectFields[i].text.Split('.');
        string extension = codeExtensionArray[codeExtensionArray.Length - 1];
        if(extension == "py")
        {
            UnityEngine.Debug.Log(clientSelectFields[i].text);
            Process.Start("/bin/bash", "-c " + "\"python3 " + clientSelectFields[i].text + "\"");
        }
        else if (extension == "java")
        {

        }
        else if (extension == "cpp")
        {

        }
        else if (extension == "go")
        {

        }
    }
}
