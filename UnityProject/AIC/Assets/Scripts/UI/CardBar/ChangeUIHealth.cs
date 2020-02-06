using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChangeUIHealth : MonoBehaviour
{
    private float normalWidth;
    private int maxHP = 90;
    // Use this for initialization
    void Start()
    {
        var rect = gameObject.GetComponent<RectTransform>();
        normalWidth = rect.rect.width;
        rect.sizeDelta = new Vector2(100, rect.rect.height);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdatePlayersStatus(List<UIPlayer> players)
    {
        var parentName = gameObject.transform.parent.parent.name;
        int player = Convert.ToInt32(parentName[parentName.Length - 1]) - 1;
        var status = players[player];
        var hp = status.Hp;
        var width = (hp / maxHP) * normalWidth;
        var rect = gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, rect.rect.height);

    }

    void SetPlayers(List<InitKing> kings)
    {
        var parentName = gameObject.transform.parent.parent.name;
        int player = Convert.ToInt32(parentName[parentName.Length - 1]) - 1;
        var hp = kings[player].Hp;
        maxHP = hp;
    }
}
