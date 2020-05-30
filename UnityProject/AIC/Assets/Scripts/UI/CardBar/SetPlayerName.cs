using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using ArabicSupport;
using UnityEngine.UI;

public class SetPlayerName : MonoBehaviour
{
    public int index;
    public void SetPlayers(List<InitKing> kings)
    {
        var text = kings[index].name;
        var textcom = gameObject.GetComponent<Text>();
        textcom.text = ArabicFixer.Fix(text);
    }
}
