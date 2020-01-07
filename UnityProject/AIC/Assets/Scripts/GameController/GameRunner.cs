using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameRunner : MonoBehaviour
{
    private class UIPlayer
    {
        public int PId { get; set; }
        public int Hp { get; set; }
        public int Ap { get; set; }
        public int[] Hand { get; set; }
        public bool isAlive { get; set; }
    }

    public float turnTime = 2.0f;
    public int unitNumbers = 9;
    public List<GameObject> playerGameObjects;

    private List<UnitAction> _unitActions;
    private List<SpellAction> _spellActions;
    private float _time = 0.0f;
    private int turnNumber = 0;
    private List<GameTurn> gameTurns;
    private int _unitActionsPointer = 0;
    private int _spellActionsPointer = 0;
    private LogParser _logParser;
    private GameUnitFactory _gameUnitFactory;

    // Start is called before the first frame update
    void Start()
    {
        var game = gameObject.GetComponent<LogReader>().ReadLog();
        gameTurns = game.Turns;
        Debug.Log(game.Init.GraphicMap.Col);
        GetComponent<MapRenderer>().RenderMap(game.Init, "FirstTile");
        _logParser = gameObject.GetComponent<LogParser>();
        _logParser.TurnTime = turnTime;
        _logParser.ParseLog(game);
        _unitActions = _logParser.UnitActions;
        _spellActions = _logParser.SpellActions;
        _gameUnitFactory = new GameUnitFactory();
    }

    // Update is called once per frame
    void Update()
    {
        while (_unitActionsPointer < _unitActions.Count && _unitActions[_unitActionsPointer].Time <= _time)
        {
            var unitAction = _unitActions[_unitActionsPointer];
            switch (unitAction.ActionType)
            {
                case UnitActionType.StartMove:
                {
                    Debug.Log("StartMove");
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    var moveController = unit.GetComponent<MoveController>();
                    var animatorController = unit.GetComponent<AnimatorController>();
                    moveController.turnTime = turnTime;
                    animatorController.SetTurnTime(turnTime);
                    animatorController.Restart();
                    animatorController.StartMoving();
                    moveController.StartMoving();
                    var attackEffectController = unit.GetComponent<AttackEffectController>();
                    if (attackEffectController != null)
                    {
                        attackEffectController.StopParticleSystem();
                    }

                    break;
                }
                case UnitActionType.MoveAfterRotate:
                {
                    Debug.Log("StartMoveAfterRotate");
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    var moveController = unit.GetComponent<MoveController>();
                    var animatorController = unit.GetComponent<AnimatorController>();
                    moveController.turnTime = turnTime;
                    animatorController.SetTurnTime(turnTime);
                    animatorController.Restart();
                    animatorController.MoveAfterRotate();
                    moveController.StartMovingAfterRotate(new Vector3(unitAction.Col, 0, unitAction.Row));
                    break;
                }
                case UnitActionType.Rotate:
                {
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    if (unit == null)
                    {
                        Debug.Log("Deploy");
                        unit = Instantiate(playerGameObjects[unitAction.PId * unitNumbers + unitAction.TypeId]
                            , new Vector3(unitAction.Col, playerGameObjects[unitAction.PId * unitNumbers + unitAction.TypeId].transform.position.y, unitAction.Row),
                            Quaternion.identity);
                        _gameUnitFactory.AddGameUnit(unitAction.UnitId, unit);
                        unit.GetComponent<AnimatorController>().Deploy();
                    }

                    Debug.Log("Rotate");
                    var moveController = unit.GetComponent<MoveController>();
                    var animatorController = unit.GetComponent<AnimatorController>();
                    moveController.turnTime = turnTime;
                    animatorController.SetTurnTime(turnTime);
                    animatorController.Restart();
                    animatorController.Rotate();
                    moveController.StartRotating(unitAction.Value - unit.transform.eulerAngles.y);
                    break;
                }
                case UnitActionType.StopMove:
                {
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    if (unit == null)
                    {
                        Debug.Log("Deploy2");
                        unit = Instantiate(playerGameObjects[unitAction.PId * unitNumbers + unitAction.TypeId]
                            , new Vector3(unitAction.Col, playerGameObjects[unitAction.PId * unitNumbers + unitAction.TypeId].transform.position.y, unitAction.Row), Quaternion.identity);
                        _gameUnitFactory.AddGameUnit(unitAction.UnitId, unit);
                        unit.GetComponent<AnimatorController>().DeployAttack();
                    }

                    Debug.Log("Attack");
                    var moveController = unit.GetComponent<MoveController>();
                    var animatorController = unit.GetComponent<AnimatorController>();
                    moveController.turnTime = turnTime;
                    animatorController.SetTurnTime(turnTime);
                    animatorController.Restart();
                    animatorController.StopMove();
                    moveController.StopEveryThing();
                    // TODO rotate to defender unit and look at it
                    // TODO play attack sound
                    var attackEffectController = unit.GetComponent<AttackEffectController>();
                    if (attackEffectController != null)
                    {
                        attackEffectController.PlayParticleSystem(_gameUnitFactory.FindById(unitAction.TargetUnitId));
                    }

                    break;
                }
                case UnitActionType.Die:
                {
                    Debug.Log("Die");
                    GameObject unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    MoveController moveController = unit.GetComponent<MoveController>();
                    var animatorController = unit.GetComponent<AnimatorController>();
                    moveController.turnTime = turnTime;
                    animatorController.SetTurnTime(turnTime);
                    animatorController.Restart();
                    animatorController.Die();
                    moveController.StopEveryThing();
                    // TODO destroy by effect
                    // TODO destroy Game Object
                    // TODO Play Die sound
                    break;
                }
            }

            _unitActionsPointer++;
        }

        while (_spellActionsPointer < _spellActions.Count && _spellActions[_spellActionsPointer].Time <= _time)
        {
            var spellAction = _spellActions[_spellActionsPointer];
            // TODO create GameSpellFactory
            if (spellAction.ActionType == SpellActionType.Put)
            {
                foreach (var sid in spellAction.UnitIds)
                {
                    GameObject unit = _gameUnitFactory.FindById(sid);
                    SpellEffectController sec = unit.GetComponent<SpellEffectController>();
                    if (sec != null)
                        sec.StartSpell(spellAction.TypeId);
                }
            }
            else
            {
                foreach (var sid in spellAction.UnitIds)
                {
                    GameObject unit = _gameUnitFactory.FindById(sid);
                    SpellEffectController sec = unit.GetComponent<SpellEffectController>();
                    if (sec != null)
                        sec.StopSpell(spellAction.TypeId);
                }
            }

            _spellActionsPointer++;
        }

        _time += Time.deltaTime;
        int newTurn = (int) Math.Truncate(_time / turnTime);
        if (newTurn != turnNumber)
        {
            turnNumber = newTurn;
            FireUIEvents(gameTurns, turnNumber);
        }
    }

    public void ChangeTurnTime(float turnTime)
    {
        this.turnTime = turnTime;
        // TODO change turn time of all components
    }

    public void FireUIEvents(List<GameTurn> gameTurns, int turnNumber)
    {
        //Debug.Log(turnNumber);
        var turn = gameTurns[turnNumber];
        gameObject.BroadcastMessage("UpdateTurnNumber", turnNumber);
        List<UIPlayer> playersStatus = new List<UIPlayer>();
        foreach (TurnPlayer tp in turn.PlayerTurnEvents)
        {
            UIPlayer player = new UIPlayer();
            player.Ap = tp.TurnEvent.Ap;
            player.Hand = tp.TurnEvent.Hand;
            player.Hp = tp.TurnEvent.Hp;
            player.isAlive = tp.TurnEvent.IsAlive;
            player.PId = tp.PId;
            playersStatus.Add(player);
        }

        gameObject.BroadcastMessage("UpdatePlayersStatus", playersStatus);
    }
}