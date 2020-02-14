using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterServer : MonoBehaviour
{
    public GameObject mainMenu, afterServer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void afterServerOnBackPressed()
    {
        afterServer.SetActive(!afterServer.activeSelf);
        mainMenu.SetActive(!afterServer.activeSelf);
    }
}
