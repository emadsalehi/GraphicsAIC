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
    
    public void FireUIEvents(GameTurn turn)
    {
        var playersStatus = turn.PlayerTurnEvents.Select(tp => new UIPlayer
            {
                Ap = tp.TurnEvent.Ap,
                Hand = tp.TurnEvent.Hand,
                Hp = tp.TurnEvent.Hp,
                isAlive = tp.TurnEvent.IsAlive,
                PId = tp.PId
            })
            .ToList();
        canvas.BroadcastMessage("UpdatePlayersStatus", playersStatus);
    }

}
