using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ServerRunFunctions : MonoBehaviour
{
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
        processStartInfo.FileName = "java";
        if (timeout == 0)
        {
            processStartInfo.Arguments = "-jar " + workingPath + "/Server/Server.jar";
        }
        else
        {
            processStartInfo.Arguments = "-jar " + workingPath + "/Server/Server.jar --extra:" + timeout;
        }

        Process serverProcess = Process.Start(processStartInfo);
        serverProcess.WaitForExit();
    }
}
