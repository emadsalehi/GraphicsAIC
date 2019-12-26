using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitActionType
{
    Deploy, StartMove, MoveAfterRotate, StopMove, Rotate, Die 
}

public class UnitAction
{
    private float Time { get; set; }
    private int Value { get; set; }
    private int UnitId { get; set; }
    private int PId { get; set; }
    private int TargetUnitId { get; set; }
    private int Row { get; set; }
    private int Col { get; set; }
    private UnitActionType ActionType { get; set; }

    public UnitAction (float time, int value, int unitId, int pId, int row, int col, int targetUnitId, UnitActionType actionType)
    {
        Time = time;
        Value = value;
        UnitId = unitId;
        PId = pId;
        ActionType = actionType;
        Row = row;
        Col = col;
        TargetUnitId = targetUnitId;
    }
}

public class LogParser : MonoBehaviour
{

    public List<UnitAction> unitActions = new List<UnitAction>();
    public float turnTime;

    public void ParseLog(Game game)
    {
        LoadUnitActions(game);
    }

    private void LoadUnitActions(Game game)
    {
                UnitFactory unitFactory = new UnitFactory();
        List<GameTurn> turns = game.Turns;

        foreach (GameTurn turn in turns)
        {
            foreach (TurnPlayer turnPlayer in turn.PlayerTurnEvents)
            {
                foreach (PlayerUnit playerUnit in turnPlayer.TurnEvent.Units)
                {
                    unitFactory.AddUnitDetail(turnPlayer.PId, playerUnit, turn.TurnNum);
                }
            }
        }
        
        foreach (UnitDetails unitDetails in unitFactory.unitDetailsList)
        {
            unitActions.Add(new UnitAction(turnTime * unitDetails.startTurn, 0, unitDetails.id
                , unitDetails.pId, unitDetails.unitEvents[0].row, unitDetails.unitEvents[0].col , 0, UnitActionType.Deploy));
            List<int> dir = new List<int> {1, 0};
            List<int> currentDir = new List<int> {0, 0};
            UnitActionType lastActionType = UnitActionType.StartMove;
            for (int i = 1; i < unitDetails.unitEvents.Count; i++)
            {
                currentDir[0] = unitDetails.unitEvents[i].row - unitDetails.unitEvents[i - 1].row;
                currentDir[1] = unitDetails.unitEvents[i].col - unitDetails.unitEvents[i - 1].col;
                if (currentDir[0] == 0 && currentDir[1] == 0)
                {
                    if (lastActionType != UnitActionType.StopMove)
                    {
                        int targetUnitId = 0;
                        List<TurnAttack> turnAttacks = game.Turns[i + unitDetails.startTurn].TurnAttacks;
                        foreach (TurnAttack turnAttack in turnAttacks)
                        {
                            if (turnAttack.AttackerId == unitDetails.id)
                            {
                                targetUnitId = turnAttack.DefenderId;
                                break;
                            }
                        }
                        lastActionType = UnitActionType.StopMove;
                        unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + i), 0, unitDetails.id
                            , unitDetails.pId, 0, 0, targetUnitId,
                            UnitActionType.StopMove));
                    }
                }
                else
                {
                    if (lastActionType == UnitActionType.StopMove)
                    {
                        lastActionType = UnitActionType.MoveAfterRotate;
                        int rotationValue = 0;
                        if (currentDir[0] == 1 && currentDir[1] == 0)
                        {
                            rotationValue = 0;
                        } else if (currentDir[0] == 0 && currentDir[1] == 1)
                        {
                            rotationValue = 90;
                        } else if (currentDir[0] == -1 && currentDir[1] == 0)
                        {
                            rotationValue = 180;
                        }
                        else
                        {
                            rotationValue = 270;
                        }
                        unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + i), rotationValue, unitDetails.id
                            , unitDetails.pId, 0, 0, 0, UnitActionType.Rotate));
                        unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + i) + turnTime / 4, 0, unitDetails.id
                            , unitDetails.pId, 0, 0, 0, UnitActionType.MoveAfterRotate));
                    }
                    else
                    {
                        if (dir[0] != currentDir[0] || dir[1] != currentDir[1])
                        {
                            int rotationValue = 0;
                            if (currentDir[0] == 1 && currentDir[1] == 0)
                            {
                                rotationValue = 0;
                            } else if (currentDir[0] == 0 && currentDir[1] == 1)
                            {
                                rotationValue = 90;
                            } else if (currentDir[0] == -1 && currentDir[1] == 0)
                            {
                                rotationValue = 180;
                            }
                            else
                            {
                                rotationValue = 270;
                            }
                            unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + i), rotationValue, unitDetails.id
                                , unitDetails.pId, 0, 0, 0, UnitActionType.Rotate));
                            unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + i) + turnTime / 4, 0, unitDetails.id
                                , unitDetails.pId, 0, 0, 0, UnitActionType.MoveAfterRotate));
                            lastActionType = UnitActionType.MoveAfterRotate;
                        }
                        else
                        {
                            unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + i), 0, unitDetails.id
                                , unitDetails.pId, 0, 0, 0, UnitActionType.StartMove));
                            lastActionType = UnitActionType.StartMove;
                        }
                    }
                }
                dir[0] = currentDir[0];
                dir[1] = currentDir[1];
            }
        }
    }
}
