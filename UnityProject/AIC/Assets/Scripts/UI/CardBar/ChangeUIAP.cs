using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChangeUIAP: MonoBehaviour
{
    private float normalWidth;
    private int maxAP = 8;
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
        var ap = status.Ap;
        var width = (ap / maxAP) * normalWidth;
        var rect = gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, rect.rect.height);

    }

    void SetMaxAP(int ap)
    {
        maxAP = ap;
    }
}
