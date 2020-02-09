using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChangeUIAP: MonoBehaviour
{
    private float _normalWidth;
    private int _maxAp = 8;
    public int player;
    
    // Use this for initialization
    void Start()
    {
        var rect = gameObject.GetComponent<RectTransform>();
        var rect1 = rect.rect;
        _normalWidth = rect1.width;
        rect.sizeDelta = new Vector2(100, rect1.height);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePlayersStatus(List<UIPlayer> players)
    {
        var status = players[player];
        var ap = status.Ap;
        var width = (ap / _maxAp) * _normalWidth;
        var rect = gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, rect.rect.height);

    }

    public void SetMaxAP(int ap)
    {
        _maxAp = ap;
    }
}
