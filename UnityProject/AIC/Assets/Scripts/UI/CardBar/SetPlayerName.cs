using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class SetPlayerName : MonoBehaviour
{
    public int index;
    public void SetPlayers(List<InitKing> kings)
    {
        var text = kings[index].Name;
        var textcom = gameObject.GetComponent<TextMeshProUGUI>();
        textcom.text = text;
    }
}
