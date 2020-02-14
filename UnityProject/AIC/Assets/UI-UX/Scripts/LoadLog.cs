using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadLog : MonoBehaviour
{
    public GameObject mainMenu, selectLog, playMenu, scrollView;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadLogMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        selectLog.SetActive(!selectLog.activeSelf);
        scrollView.GetComponent<DynamicScrollView>().LoadLocalLogFiles();
    }

    public void LoadLogOnBackPressed()
    {
        selectLog.SetActive(!selectLog.activeSelf);
        mainMenu.SetActive(!selectLog.activeSelf);
    }

    

}
