using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class SetPlayerName : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetPlayers(List<InitKing> kings)
    {
        var parentName = gameObject.transform.parent.parent.name;
        int index = Convert.ToInt32(parentName[parentName.Length - 1]) - 1;
        var text = kings[index].Name;
        var textcom = gameObject.GetComponent<TextMeshProUGUI>();
        textcom.text = text;
    }
}
