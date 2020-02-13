using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EndGameDetailsLoader : MonoBehaviour
{
    public GameObject player1, player2, player3, player4;
    public GameObject point1, point2, point3, point4;
    public GameObject win, lose, draw;
    public GameObject hLine;

    private List<string> names = new List<string>();

    public void InitializeNames(List<InitKing> kings)
    {
        foreach (var king in kings)
        {
            names.Add(king.Name);
        }
    }
    
    public void CreateEndGame(List<EndGame> end)
    {
        if (end.ElementAt(0).Score + end.ElementAt(2).Score == end.ElementAt(1).Score + end.ElementAt(3).Score)
        {
            win.SetActive(false);
            lose.SetActive(false);
            hLine.SetActive(false);
            draw.SetActive(true);
        }
        else
        {
            win.SetActive(true);
            lose.SetActive(true);
            hLine.SetActive(true);
            draw.SetActive(false);
        }
        var sorted = end.OrderByDescending(o => o.Score).ToList();
        player1.GetComponent<TextMeshProUGUI>().text = names.ElementAt(sorted[0].PlayerId);
        player2.GetComponent<TextMeshProUGUI>().text = names.ElementAt(sorted[1].PlayerId);
        player3.GetComponent<TextMeshProUGUI>().text = names.ElementAt(sorted[2].PlayerId);
        player4.GetComponent<TextMeshProUGUI>().text = names.ElementAt(sorted[3].PlayerId);
        point1.GetComponent<TextMeshProUGUI>().text = sorted[0].Score.ToString();
        point2.GetComponent<TextMeshProUGUI>().text = sorted[1].Score.ToString();
        point3.GetComponent<TextMeshProUGUI>().text = sorted[2].Score.ToString();
        point4.GetComponent<TextMeshProUGUI>().text = sorted[3].Score.ToString();
    }
}
