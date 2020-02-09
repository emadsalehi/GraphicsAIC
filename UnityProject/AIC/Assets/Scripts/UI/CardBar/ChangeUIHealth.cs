using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChangeUIHealth : MonoBehaviour
{
    private float _normalWidth;
    private int _maxHp = 90;
    public int player;
    // Use this for initialization
    void Start()
    {
        var rect = gameObject.GetComponent<RectTransform>();
        _normalWidth = rect.rect.width;
        rect.sizeDelta = new Vector2(100, rect.rect.height);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePlayersStatus(List<UIPlayer> players)
    {
        var status = players[player];
        var hp = status.Hp;
        var width = (hp / _maxHp) * _normalWidth;
        var rect = gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, rect.rect.height);

    }

    public void SetPlayers(List<InitKing> kings)
    {
        var parentName = gameObject.transform.parent.parent.name;
        int player = Convert.ToInt32(parentName[parentName.Length - 1]) - 1;
        var hp = kings[player].Hp;
        _maxHp = hp;
    }
}
