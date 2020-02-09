using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ChangeHeroImages : MonoBehaviour
{
    public Sprite[] images;
    public int index;
    public int player;
    
    // Use this for initialization
    void Start()
    {
        var img = gameObject.GetComponent<Image>();
    }

    void UpdatePlayersStatus(List<UIPlayer> hands)
    {
        var hand = hands[player];
        var imgindex = hand.Hand[index];
        var img = gameObject.GetComponent<Image>();
        img.sprite = images[imgindex];
    }
}
