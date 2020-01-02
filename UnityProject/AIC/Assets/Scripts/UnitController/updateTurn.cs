using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateTurn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var textcom = gameObject.GetComponent("Text") as UnityEngine.UI.Text;
        textcom.text = "hoora";
        Debug.Log(textcom.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
