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
    public UnitActionType ActionType { get; set; }

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

public class SpellAction{
    public float Time { get; set; }
    public int TypeId { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
    public int Range{ get; set; }
    public SpellActionType ActionType { get; set; }
    public int SpellId{get; set; }
    
    public SpellAction(float time ,int typeId ,int row ,int col ,int range, SpellActionType spellActionType, int spellId )
    {
        Time = time;
        TypeId = typeId;
        Row = row;
        Col = col;
        Range = range;
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
        LoadUnitActions(game);
        LoadSpellActions(game);
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
            List<int> dir = new List<int> {1, 0};
            List<int> currentDir = new List<int> {0, 0};
            UnitActionType lastActionType = UnitActionType.Deploy;
            for (int i = 0; i < unitDetails.unitEvents.Count - 1; i++)
            {
                currentDir[0] = unitDetails.unitEvents[i + 1].row - unitDetails.unitEvents[i].row;
                currentDir[1] = unitDetails.unitEvents[i + 1].col - unitDetails.unitEvents[i].col;
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
                    if (lastActionType == UnitActionType.StopMove || lastActionType == UnitActionType.Deploy)
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
                            , unitDetails.pId, currentDir[0], currentDir[1], 0, UnitActionType.MoveAfterRotate));
                    }
                    else
                    {
                        if (dir[0] != currentDir[0] || dir[1] != currentDir[1] || i == 0)
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
                                , unitDetails.pId, currentDir[0], currentDir[1], 0, UnitActionType.MoveAfterRotate));
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
            unitActions.Add(new UnitAction(turnTime * (unitDetails.startTurn + unitDetails.unitEvents.Count - 1)
                , 0, unitDetails.id, unitDetails.pId, 0, 0, 0, UnitActionType.Die));
        }
        unitActions = unitActions.OrderBy(o => o.Time).ToList();
    }

    private void LoadSpellActions(Game game)
    {
        SpellFactory spellFactory = new SpellFactory() ;
        List<GameTurn> turns = game.Turns;
        foreach (GameTurn turn in turns)
        {
            foreach (TurnPlayer turnPlayer in turn.PlayerTurnEvents)  
            {
                foreach (PlayerMapSpell playerMapSpell in turnPlayer.TurnEvent.MapSpells )
                {

                    spellFactory.AddSpellDetails(turnPlayer.PId , playerMapSpell , turn.TurnNum , playerMapSpell.Center.Row , playerMapSpell.Center.Col , playerMapSpell.TypeId , playerMapSpell.Range);
                }

            }    
        }
        foreach (SpellDetails spellDetails in spellFactory.spellDetailsList)
        {
            spellActions.Add(new SpellAction(turnTime * spellDetails.startTurn , spellDetails.id , spellDetails.row ,
             spellDetails.col, spellDetails.range ,SpellActionType.Put , spellDetails.id));
            spellActions.Add(new SpellAction(turnTime * (spellDetails.startTurn + spellDetails.aliveTurns) ,
             spellDetails.id , spellDetails.row , spellDetails.col, spellDetails.range ,SpellActionType.Pick , spellDetails.id ));

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
