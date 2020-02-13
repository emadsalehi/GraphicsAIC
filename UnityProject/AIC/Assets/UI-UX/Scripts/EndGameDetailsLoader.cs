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
            names.Add(king.name);
        }
    }
    
    public void CreateEndGame(List<EndGame> end)
    {
        if (end.ElementAt(0).score + end.ElementAt(2).score == end.ElementAt(1).score + end.ElementAt(3).score)
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
        var sorted = end.OrderByDescending(o => o.score).ToList();
        player1.GetComponent<TextMeshProUGUI>().text = names.ElementAt(sorted[0].playerId);
        player2.GetComponent<TextMeshProUGUI>().text = names.ElementAt(sorted[1].playerId);
        player3.GetComponent<TextMeshProUGUI>().text = names.ElementAt(sorted[2].playerId);
        player4.GetComponent<TextMeshProUGUI>().text = names.ElementAt(sorted[3].playerId);
        point1.GetComponent<TextMeshProUGUI>().text = sorted[0].score.ToString();
        point2.GetComponent<TextMeshProUGUI>().text = sorted[1].score.ToString();
        point3.GetComponent<TextMeshProUGUI>().text = sorted[2].score.ToString();
        point4.GetComponent<TextMeshProUGUI>().text = sorted[3].score.ToString();
    }
}
