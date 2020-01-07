using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UnitActionType
{
    Deploy, StartMove, MoveAfterRotate, StopMove, Rotate, Die 
}

public enum SpellActionType
{
    Put, Pick
}

public class UnitAction
{
    public float Time { get; }
    public int Value { get; set; }
    public int UnitId { get; set; }
    public int PId { get; set; }
    public int TargetUnitId { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
    public int TypeId { get; set; }
    public UnitActionType ActionType { get; set; }

    public UnitAction (float time, int value, int unitId, int pId, int row
        , int col, int targetUnitId, int typeId, UnitActionType actionType)
    {
        Time = time;
        Value = value;
        UnitId = unitId;
        PId = pId;
        ActionType = actionType;
        Row = row;
        Col = col;
        TargetUnitId = targetUnitId;
        TypeId = typeId;
    }
}

public class SpellAction{
    public float Time { get; set; }
    public int TypeId { get; set; }
    public List<int> UnitIds { get; set; } 
    public SpellActionType ActionType { get; set; }
    public int SpellId{get; set; }
    
    public SpellAction(float time ,int typeId ,List<int> unitIds , SpellActionType spellActionType, int spellId )
    {
        Time = time;
        TypeId = typeId;
        UnitIds = unitIds;
        ActionType = spellActionType;
        SpellId = spellId;

    }
}


public class LogParser : MonoBehaviour
{
    private List<UnitAction> unitActions = new List<UnitAction>();
    private List<SpellAction> spellActions = new List<SpellAction>();
    private float turnTime;

    public LogParser(float turnTime)
    {
        this.turnTime = turnTime;
    }

    public void ParseLog(Game game)
    {
        unitActions = new List<UnitAction>();
        spellActions = new List<SpellAction>();
        LoadUnitActions(game);
        LoadSpellActions(game);
    }

    private void LoadUnitActions(Game game)
    {
        var unitFactory = new UnitFactory();
        var turns = game.Turns;

        foreach (var turn in turns)
        {
            foreach (var turnPlayer in turn.PlayerTurnEvents)
            {
                if (turnPlayer.TurnEvent.Units == null) continue;
                foreach (var playerUnit in turnPlayer.TurnEvent.Units)
                {
                    unitFactory.AddUnitDetail(turnPlayer.PId, playerUnit, turn.TurnNum);
                }
            }
        }
        
        foreach (var unitDetails in unitFactory.unitDetailsList)
        {
            var dir = new List<int> {1, 0};
            var currentDir = new List<int> {0, 0};
            var lastActionType = UnitActionType.Deploy;
            for (var i = 0; i < unitDetails.unitEvents.Count - 1; i++)
            {
                currentDir[0] = unitDetails.unitEvents[i + 1].row - unitDetails.unitEvents[i].row;
                currentDir[1] = unitDetails.unitEvents[i + 1].col - unitDetails.unitEvents[i].col;
                if (currentDir[0] == 0 && currentDir[1] == 0)
                {
                    if (lastActionType != UnitActionType.StopMove)
                    {
                        var targetUnitId = 0;
                        var turnAttacks = game.Turns[i + unitDetails.startTurn].TurnAttacks;
                        foreach (var turnAttack in turnAttacks)
                        {
                            if (turnAttack.AttackerId != unitDetails.id) continue;
                            targetUnitId = turnAttack.DefenderId;
                            break;
                        }
                        lastActionType = UnitActionType.StopMove;
                        unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + i), 0, unitDetails.id
                            , unitDetails.pId, 0, 0, targetUnitId, unitDetails.typeId, UnitActionType.StopMove));
                    }
                }
                else
                {
                    if (lastActionType == UnitActionType.StopMove || lastActionType == UnitActionType.Deploy)
                    {
                        var rotationValue = 0;
                        switch (currentDir[0])
                        {
                            case 1 when currentDir[1] == 0:
                                rotationValue = 0;
                                break;
                            case 0 when currentDir[1] == 1:
                                rotationValue = 90;
                                break;
                            case -1 when currentDir[1] == 0:
                                rotationValue = 180;
                                break;
                            default:
                                rotationValue = 270;
                                break;
                        }
                        unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + i), rotationValue, unitDetails.id
                            , unitDetails.pId, unitDetails.unitEvents[i].row, unitDetails.unitEvents[i].col, 0, unitDetails.typeId, UnitActionType.Rotate));
                        unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + i) + turnTime / 3.5f, 0, unitDetails.id
                            , unitDetails.pId, currentDir[0], currentDir[1], 0, unitDetails.typeId, UnitActionType.MoveAfterRotate));
                        lastActionType = UnitActionType.MoveAfterRotate;
                    }
                    else
                    {
                        if (dir[0] != currentDir[0] || dir[1] != currentDir[1])
                        {
                            var rotationValue = 0;
                            switch (currentDir[0])
                            {
                                case 1 when currentDir[1] == 0:
                                    rotationValue = 0;
                                    break;
                                case 0 when currentDir[1] == 1:
                                    rotationValue = 90;
                                    break;
                                case -1 when currentDir[1] == 0:
                                    rotationValue = 180;
                                    break;
                                default:
                                    rotationValue = 270;
                                    break;
                            }
                            unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + i), rotationValue, unitDetails.id
                                , unitDetails.pId, 0, 0, 0, unitDetails.typeId, UnitActionType.Rotate));
                            unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + i) + turnTime / 3.5f, 0, unitDetails.id
                                , unitDetails.pId, currentDir[0], currentDir[1], 0, unitDetails.typeId, UnitActionType.MoveAfterRotate));
                            lastActionType = UnitActionType.MoveAfterRotate;
                        }
                        else
                        {
                            unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + i), 0, unitDetails.id
                                , unitDetails.pId, 0, 0, 0, unitDetails.typeId, UnitActionType.StartMove));
                            lastActionType = UnitActionType.StartMove;
                        }
                    }
                }
                dir[0] = currentDir[0];
                dir[1] = currentDir[1];
            }
            unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + unitDetails.unitEvents.Count - 1)
                , 0, unitDetails.id, unitDetails.pId, 0, 0, 0, unitDetails.typeId, UnitActionType.Die));
        }
        unitActions = unitActions.OrderBy(o => o.Time).ToList();
    }

    private void LoadSpellActions(Game game)
    {
        var spellFactory = new SpellFactory() ;
        var turns = game.Turns;
        foreach (var turn in turns)
        {
            foreach (var turnPlayer in turn.PlayerTurnEvents)
            {
                if (turnPlayer.TurnEvent.MapSpells == null) continue;
                foreach (var playerMapSpell in turnPlayer.TurnEvent.MapSpells)
                {
                    spellFactory.AddSpellDetails(turnPlayer.PId, playerMapSpell, turn.TurnNum, playerMapSpell.SpellId);
                }
            }    
        }
        foreach (var spellDetails in spellFactory.spellDetailsList)
        {
            spellActions.Add(new SpellAction(turnTime * spellDetails.startTurn , spellDetails.id , spellDetails.unitIds ,SpellActionType.Put , spellDetails.id));
            spellActions.Add(new SpellAction(turnTime * (spellDetails.startTurn + spellDetails.aliveTurns) ,
             spellDetails.id , spellDetails.unitIds ,SpellActionType.Pick , spellDetails.id ));

        }

        spellActions = spellActions.OrderBy(o => o.Time).ToList();
    }

    public List<UnitAction> UnitActions
    {
        get => unitActions;
        set => unitActions = value;
    }

    public List<SpellAction> SpellActions
    {
        get => spellActions;
        set => spellActions = value;
    }

    public float TurnTime
    {
        get => turnTime;
        set => turnTime = value;
    }
}
