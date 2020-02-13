using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UnitActionType
{
    Deploy,
    StartMove,
    MoveAfterRotate,
    StopMove,
    Rotate,
    Die,
    Teleport,
    Haste,
    Destroy
}

public enum SpellActionType
{
    Put,
    Pick
}

public enum KingActionType
{
    Attack,
    Die,
    ChangeTarget,
    StopAttack
}

public class UnitAction
{
    public float Time { get; set; }
    public float Value { get; set; }
    public int UnitId { get; set; }
    public int PId { get; set; }
    public int TargetUnitId { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
    public int TypeId { get; set; }
    public UnitActionType ActionType { get; set; }

    public UnitAction(float time, float value, int unitId, int pId, int row
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

public class SpellAction
{
    public float Time { get; set; }
    public int TypeId { get; set; }
    public List<int> UnitIds { get; set; }
    public SpellActionType ActionType { get; set; }
    public int SpellId { get; set; }

    public SpellAction(float time, int typeId, List<int> unitIds, SpellActionType spellActionType, int spellId)
    {
        Time = time;
        TypeId = typeId;
        UnitIds = unitIds;
        ActionType = spellActionType;
        SpellId = spellId;
    }
}

public class KingAction
{
    public float Time { get; set; }
    public int PId { get; set; }
    public int TargetUnitId { get; set; }
    public KingActionType KingActionType { get; set; }

    public KingAction(float time, int pId, int targetUnitId, KingActionType kingActionType)
    {
        Time = time;
        PId = pId;
        TargetUnitId = targetUnitId;
        KingActionType = kingActionType;
    }
}

public class LogParser : MonoBehaviour
{
    private List<UnitAction> _unitActions = new List<UnitAction>();
    private List<SpellAction> _spellActions = new List<SpellAction>();
    private List<KingAction> _kingActions = new List<KingAction>();
    private float _turnTime;

    public LogParser(float turnTime)
    {
        _turnTime = turnTime;
    }

    public void ParseLog(Game game)
    {
        _unitActions = new List<UnitAction>();
        _spellActions = new List<SpellAction>();
        _kingActions = new List<KingAction>();
        LoadUnitActions(game);
        LoadSpellActions(game);
        LoadKingActions(game);
    }

    private void LoadKingActions(Game game)
    {
        var lastKingTarget = new int[4];
        var isAttacking = new int[4];
        var isDied = new bool[4];

        for (var i = 0; i < game.Turns.Count; i++)
        {
            var turn = game.Turns[i];
            foreach (var turnPlayer in turn.PlayerTurnEvents)
            {
                if (turnPlayer.TurnEvent.Hp <= 0 && !isDied[turnPlayer.PId])
                {
                    _kingActions.Add(new KingAction(_turnTime * i, turnPlayer.PId, 0, KingActionType.Die));
                    isDied[turnPlayer.PId] = true;
                }
            }

            for (var j = 0; j < 4; j++)
            {
                var found = false;
                foreach (var turnAttack in from turnAttack in turn.TurnAttacks
                    let attackerId = turnAttack.AttackerId
                    where attackerId == j
                    select turnAttack)
                {
                    isAttacking[j] = turnAttack.DefenderId;
                    found = true;
                }

                if (!found)
                {
                    isAttacking[j] = 0;
                }

                if (isAttacking[j] == 0)
                {
                    if (lastKingTarget[j] == 0) continue;
                    _kingActions.Add(new KingAction(_turnTime * i, 0, 0, KingActionType.StopAttack));
                    lastKingTarget[j] = 0;
                }
                else
                {
                    if (lastKingTarget[j] == 0)
                        _kingActions.Add(new KingAction(_turnTime * i, j, isAttacking[j], KingActionType.Attack));
                    else if (lastKingTarget[j] != isAttacking[j])
                        _kingActions.Add(new KingAction(_turnTime * i, j, isAttacking[j], KingActionType.ChangeTarget));
                }
            }
        }
    }

    private void LoadUnitActions(Game game)
    {
        var unitFactory = new UnitFactory();
        var turns = game.Turns;

        for (var i = 0; i < turns.Count; i++)
        {
            var turn = turns[i];
            foreach (var turnPlayer in turn.PlayerTurnEvents)
            {
                if (turnPlayer.TurnEvent.Units == null) continue;
                foreach (var playerUnit in turnPlayer.TurnEvent.Units)
                {
                    unitFactory.AddUnitDetail(turnPlayer.PId, playerUnit, i);
                }
            }
        }

        foreach (var unitDetails in unitFactory.unitDetailsList)
        {
            var dir = new List<int> {0, 0};
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
                        var turnAttacks = game.Turns[i + unitDetails.startTurn + 1].TurnAttacks;
                        var targetUnitId =
                            (from turnAttack in turnAttacks
                                where turnAttack.AttackerId == unitDetails.id
                                select turnAttack.DefenderId).FirstOrDefault();

                        lastActionType = UnitActionType.StopMove;
                        _unitActions.Add(new UnitAction(_turnTime * (unitDetails.startTurn + i), 0, unitDetails.id
                            , unitDetails.pId, unitDetails.unitEvents[i].row, unitDetails.unitEvents[i].col,
                            targetUnitId, unitDetails.typeId, UnitActionType.StopMove));
                    }
                }
                else
                {
                    if (!(lastActionType == UnitActionType.StopMove || lastActionType == UnitActionType.Deploy ||
                          lastActionType == UnitActionType.Teleport || lastActionType == UnitActionType.Haste ||
                          lastActionType == UnitActionType.StartMove))
                    {
                        _unitActions.Add(new UnitAction(_turnTime * (unitDetails.startTurn + i), 1.0f, unitDetails.id
                            , unitDetails.pId, 0, 0, 0, unitDetails.typeId, UnitActionType.StartMove));
                        lastActionType = UnitActionType.StartMove;
                    }

                    if (currentDir[0] > 1 || currentDir[0] < -1 || currentDir[1] > 1 || currentDir[1] < -1)
                    {
                        var turnNum = unitDetails.startTurn + i;
                        foreach (var mapSpell in game.Turns[turnNum + 1].PlayerTurnEvents[unitDetails.pId].TurnEvent
                            .MapSpells)
                        {
                            if (mapSpell.TypeId == 0)
                            {
                                var speed = (float) Math.Sqrt(
                                    Math.Pow(currentDir[0], 2) + Math.Pow(currentDir[1], 2));
                                _unitActions.Add(new UnitAction(_turnTime * (unitDetails.startTurn + i), speed,
                                    unitDetails.id
                                    , unitDetails.pId, currentDir[0], currentDir[1],
                                    0, unitDetails.typeId, UnitActionType.Haste));
                                lastActionType = UnitActionType.Haste;
                                // TODO: haste checking
                            }
                            else
                            {
                                _unitActions.Add(new UnitAction(_turnTime * (unitDetails.startTurn + i), 0,
                                    unitDetails.id
                                    , unitDetails.pId, unitDetails.unitEvents[i + 1].row,
                                    unitDetails.unitEvents[i + 1].col,
                                    0, unitDetails.typeId,
                                    UnitActionType.Teleport));
                                lastActionType = UnitActionType.Teleport;
                            }
                        }
                    }
                    else if (dir[0] != currentDir[0] || dir[1] != currentDir[1] ||
                             lastActionType == UnitActionType.Haste || i == 0)
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

                        _unitActions.Add(new UnitAction(_turnTime * (unitDetails.startTurn + i), rotationValue,
                            unitDetails.id
                            , unitDetails.pId, unitDetails.unitEvents[i].row, unitDetails.unitEvents[i].col, 0,
                            unitDetails.typeId, UnitActionType.Rotate));
                        _unitActions.Add(new UnitAction(_turnTime * (unitDetails.startTurn + i) + _turnTime / 3.5f,
                            1.0f,
                            unitDetails.id
                            , unitDetails.pId, currentDir[0], currentDir[1], 0, unitDetails.typeId,
                            UnitActionType.MoveAfterRotate));
                        lastActionType = UnitActionType.MoveAfterRotate;
                    }
                }

                dir[0] = currentDir[0];
                dir[1] = currentDir[1];
            }

            _unitActions.Add(new UnitAction(_turnTime * (unitDetails.startTurn + unitDetails.unitEvents.Count - 1)
                , 0, unitDetails.id, unitDetails.pId, 0, 0, 0, unitDetails.typeId, UnitActionType.Die));
            _unitActions.Add(new UnitAction(_turnTime * (unitDetails.startTurn + unitDetails.unitEvents.Count)
                , 0, unitDetails.id, unitDetails.pId, 0, 0, 0, unitDetails.typeId, UnitActionType.Destroy));
        }

        _unitActions = _unitActions.OrderBy(o => o.Time).ToList();
    }

    private void LoadSpellActions(Game game)
    {
        var spellFactory = new SpellFactory();
        var turns = game.Turns;
        for (var i = 0; i < turns.Count; i++)
        {
            var turn = turns[i];
            foreach (var turnPlayer in turn.PlayerTurnEvents)
            {
                if (turnPlayer.TurnEvent.MapSpells == null) continue;
                foreach (var playerMapSpell in turnPlayer.TurnEvent.MapSpells)
                {
                    spellFactory.AddSpellDetails(turnPlayer.PId, playerMapSpell, i - 1,
                        playerMapSpell.SpellId);
                }
            }
        }

        foreach (var spellDetails in spellFactory.spellDetailsList)
        {
            _spellActions.Add(new SpellAction(_turnTime * spellDetails.startTurn, spellDetails.typeId,
                spellDetails.unitIds, SpellActionType.Put, spellDetails.id));
            _spellActions.Add(new SpellAction(_turnTime * (spellDetails.startTurn + spellDetails.aliveTurns),
                spellDetails.typeId, spellDetails.unitIds, SpellActionType.Pick, spellDetails.id));
        }

        _spellActions = _spellActions.OrderBy(o => o.Time).ToList();
    }

    public List<UnitAction> UnitActions => _unitActions;

    public List<SpellAction> SpellActions => _spellActions;

    public List<KingAction> KingActions => _kingActions;

    public float TurnTime
    {
        get => _turnTime;
        set => _turnTime = value;
    }
}