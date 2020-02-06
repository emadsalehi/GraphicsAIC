using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ChangeHeroImages : MonoBehaviour
{
    public Sprite[] images;
    // Use this for initialization
    void Start()
    {
        var img = gameObject.GetComponent<Image>();
        img.sprite = images[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdatePlayersStatus(List<UIPlayer> hands)
    {
        int player = Convert.ToInt32(gameObject.transform.parent.name[gameObject.transform.parent.name.Length - 1]) - 1;
        int index = Convert.ToInt32(gameObject.name[gameObject.name.Length - 1]) - 1;
        var hand = hands[player];
        var imgindex = hand.Hand[index];
        var img = gameObject.GetComponent<Image>();
        img.sprite = images[imgindex];
    }
}
