using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.IO;

public class ServerRunFunctions : MonoBehaviour
{
    public string map_path;

    public GameObject afterServer, serverRun;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartProcess(int timeout)
    {
        
        string workingPath = Application.dataPath;
        ProcessStartInfo processStartInfo = new ProcessStartInfo();
        processStartInfo.CreateNoWindow = true;
        processStartInfo.UseShellExecute = false;
        string currentDirectory = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(workingPath+"/Server/");
        #if UNITY_EDITOR
            processStartInfo.FileName = "java";
        #endif
        
        #if UNITY_STANDALONE_OSX
            processStartInfo.FileName = "java";
        #endif
        #if UNITY_STANDALONE_WIN
            processStartInfo.FileName = "java.exe";
        #endif
        #if UNITY_STANDALONE_LINUX
            processStartInfo.FileName = "java";
        #endif
        if (timeout == 0)
        {
            processStartInfo.Arguments = "-jar " + "server.jar";
        }
        else
        {
            processStartInfo.Arguments = "-jar " + "server.jar --extra:" + timeout;
        }
        
        Process serverProcess = Process.Start(processStartInfo);
        // serverProcess.WaitForExit();
        Directory.SetCurrentDirectory(currentDirectory);
        serverRun.SetActive(!serverRun.activeSelf);
        afterServer.SetActive(!afterServer.activeSelf);
    }
}
