using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIContoller : MonoBehaviour
{
    public GameObject canvas;

    public void UpdateTurnNumberBroadcast(int turnNumber)
    {
        canvas.BroadcastMessage("UpdateTurnNumber", turnNumber);
    }

    public void UpdateSlider(int turnNumber, int totalTurnNumber)
    {
        canvas.BroadcastMessage("UpdateSliderValue", (float) turnNumber / totalTurnNumber);
    }

    public void FireEndGameEvents(List<EndGame> end, List<InitKing> kings)
    {
        canvas.BroadcastMessage("LoadEndGame");
        canvas.BroadcastMessage("InitializeNames", kings);
        canvas.BroadcastMessage("CreateEndGame", end);
    }
    
    public void FireUiEvents(GameTurn turn)
    {
        var playersStatus = turn.playerTurnEvents.Select(tp => new UIPlayer
            {
                Ap = tp.turnEvent.ap,
                Hand = tp.turnEvent.hand,
                Hp = tp.turnEvent.hp,
                isAlive = tp.turnEvent.isAlive,
                PId = tp.pId
            })
            .ToList();
        canvas.BroadcastMessage("UpdatePlayersStatus", playersStatus);
    }

}
