using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LoadLog : MonoBehaviour
{
    public GameObject mainMenu, selectLog, playMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadLogMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        selectLog.SetActive(!selectLog.activeSelf);
    }

}
