using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Debug.Log(mousePos);
            //mousePos.y = 0;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
            Debug.Log("mouse" + mousePosition);
            Debug.Log(Input.mousePosition);
        }
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

    }
    
}
