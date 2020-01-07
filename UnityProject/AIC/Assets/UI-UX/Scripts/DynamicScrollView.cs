using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class DynamicScrollView : MonoBehaviour
{
    public GameObject Prefab;
    public Transform Container;
    public List<string> files = new List<string>();

    void Start()
    {
        String[] log_files = { "Ashkan", "Emad", "Shayesteh", "Sina"};

        for (int i = 0; i < log_files.Length; i++)
        {
            GameObject go = Instantiate(Prefab);
            go.GetComponentInChildren<Text>().text = log_files[i];
            go.transform.SetParent(Container);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            int buttonIndex = i;
            go.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(buttonIndex));
        }
    }



    public void OnButtonClick(int index)
    {
        string file = files[index];

        Debug.Log(file);
        // Process file here...
    }
}