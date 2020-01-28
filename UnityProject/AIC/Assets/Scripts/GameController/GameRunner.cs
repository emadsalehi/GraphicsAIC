using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class GameRunner : MonoBehaviour
{
    public float turnTime = 2.0f;
    public int unitNumbers = 9;
    public List<GameObject> playerGameObjects;

    private List<UnitAction> _unitActions;
    private List<SpellAction> _spellActions;
    private List<GameTurn> _gameTurns;
    private List<GameObject> _towers = new List<GameObject>();
    private GameUnitFactory _gameUnitFactory;
    private LogParser _logParser;
    private AudioManager _audioManager;
    private float _time = 0.0f;
    private float _uiTime = 0.0f;
    private float _timeSpeed = 1.0f;
    private const float TileSize = 1.0f;
    private int _turnNumber = 0;
    private int _unitActionsPointer = 0;
    private int _spellActionsPointer = 0;

    // Start is called before the first frame update
    void Start()
    {
        var game = gameObject.GetComponent<LogReader>().ReadLog();
        _gameTurns = game.Turns;
        GetComponent<MapRenderer>().RenderMap(game.Init, "FirstTile");
        _logParser = gameObject.GetComponent<LogParser>();
        _logParser.TurnTime = turnTime;
        _logParser.ParseLog(game);
        _unitActions = _logParser.UnitActions;
        _spellActions = _logParser.SpellActions;
        _gameUnitFactory = new GameUnitFactory();
        if (GameObject.Find("SoundController") != null)
        {
            _audioManager = GameObject.Find("SoundController").GetComponent<AudioManager>();
            _audioManager.Stop("Menu");
            _audioManager.Play("Game");
        }
        _towers.Add(GameObject.FindWithTag("Tower1"));
        _towers.Add(GameObject.FindWithTag("Tower2"));
        _towers.Add(GameObject.FindWithTag("Tower3"));
        _towers.Add(GameObject.FindWithTag("Tower4"));
    }

    // Update is called once per frame
    void Update()
    {
        ApplyUnitActions();
        ApplySpellActions();
        _time += Time.deltaTime * _timeSpeed;
        _uiTime += Time.deltaTime;

        var newTurn = (int) Math.Truncate(_uiTime / turnTime);
        if (newTurn == _turnNumber) return;
        _turnNumber = newTurn;
        FireUIEvents(_gameTurns, _turnNumber);
    }

    public void ChangeTurnTime(float turnTime)
    {
        _timeSpeed *= (this.turnTime / turnTime);
        _uiTime /= (this.turnTime / turnTime);
        this.turnTime = turnTime;
        var units = _gameUnitFactory.GetAllUnits();
        foreach (var unit in units)
        {
            unit.GetComponent<MoveController>().turnTime = turnTime;
            unit.GetComponent<AnimatorController>().SetTurnTime(turnTime);
        }
    }

    public void PauseGameRunner()
    {
        Time.timeScale = 0.0f;
    }

    public void PlayGameRunner()
    {
        Time.timeScale = 1.0f;
    }

    private void ApplyUnitActions()
    {
        while (_unitActionsPointer < _unitActions.Count && _unitActions[_unitActionsPointer].Time <= _time)
        {
            var unitAction = _unitActions[_unitActionsPointer];
            switch (unitAction.ActionType)
            {
                case UnitActionType.StartMove:
                {
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    var moveController = unit.GetComponent<MoveController>();
                    var animatorController = unit.GetComponent<AnimatorController>();
                    animatorController.Restart();
                    animatorController.StartMoving();
                    moveController.StartMoving();
                    break;
                }
                case UnitActionType.MoveAfterRotate:
                {
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    var moveController = unit.GetComponent<MoveController>();
                    moveController.StartMovingAfterRotate(new Vector3(unitAction.Col, 0, unitAction.Row));
                    break;
                }
                case UnitActionType.Rotate:
                {
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    if (unit == null)
                    {
                        Debug.Log("Deploy on " + unitAction.UnitId + " on turn " + _turnNumber);
                        var xOffset = Random.Range(-TileSize / 3, TileSize / 3);
                        var zOffset = Random.Range(-TileSize / 3, TileSize / 3);
                        unit = Instantiate(playerGameObjects[unitAction.PId * unitNumbers + unitAction.TypeId]
                            , new Vector3(unitAction.Col + xOffset, playerGameObjects[unitAction.PId * unitNumbers + unitAction.TypeId].transform.position.y, unitAction.Row + zOffset),
                            Quaternion.identity);
                        _gameUnitFactory.AddGameUnit(unitAction.UnitId, unit);
                        unit.GetComponent<AnimatorController>().Deploy();
                        var moveController1 = unit.GetComponent<MoveController>();
                        var animatorController1 = unit.GetComponent<AnimatorController>();
                        moveController1.turnTime = turnTime;
                        animatorController1.SetTurnTime(turnTime);
                    }
                    var moveController = unit.GetComponent<MoveController>();
                    var animatorController = unit.GetComponent<AnimatorController>();
                    animatorController.Restart();
                    animatorController.Rotate();
                    moveController.StartRotating(unitAction.Value - unit.transform.eulerAngles.y);
                    unit.GetComponent<AudioSource>().Stop();
                    var attackEffectController = unit.GetComponent<AttackEffectController>();
                    if (attackEffectController != null)
                    {
                        attackEffectController.StopParticleSystem();
                    }

                    break;
                }
                case UnitActionType.StopMove:
                {
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    if (unit == null)
                    {
                        Debug.Log("Deploy2 on " + unitAction.UnitId + " on turn " + _turnNumber);
                        var xOffset = Random.Range(-TileSize / 3, TileSize / 3);
                        var zOffset = Random.Range(-TileSize / 3, TileSize / 3);
                        unit = Instantiate(playerGameObjects[unitAction.PId * unitNumbers + unitAction.TypeId]
                            , new Vector3(unitAction.Col + xOffset, playerGameObjects[unitAction.PId * unitNumbers + unitAction.TypeId].transform.position.y, unitAction.Row + zOffset),
                            Quaternion.identity);
                        _gameUnitFactory.AddGameUnit(unitAction.UnitId, unit);
                        unit.GetComponent<AnimatorController>().DeployAttack();
                        var moveController1 = unit.GetComponent<MoveController>();
                        var animatorController1 = unit.GetComponent<AnimatorController>();
                        moveController1.turnTime = turnTime;
                        animatorController1.SetTurnTime(turnTime);
                    }
                    var moveController = unit.GetComponent<MoveController>();
                    var animatorController = unit.GetComponent<AnimatorController>();
                    animatorController.Restart();
                    animatorController.StopMove();
                    moveController.StopEveryThing();
                    var target = unitAction.TargetUnitId < 4 && unitAction.TargetUnitId >= 0
                        ? _towers[unitAction.TargetUnitId]
                        : _gameUnitFactory.FindById(unitAction.TargetUnitId);
                    moveController.Attack(target);
                    unit.GetComponent<AudioSource>().Play();
                    var attackEffectController = unit.GetComponent<AttackEffectController>();
                    if (attackEffectController != null)
                    {
                        attackEffectController.PlayParticleSystem(target);
                    }

                    break;
                }
                case UnitActionType.Die:
                {
                        var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    var moveController = unit.GetComponent<MoveController>();
                    var animatorController = unit.GetComponent<AnimatorController>();
                    animatorController.Restart();
                    animatorController.Die();
                    moveController.StopEveryThing();
                    var attackEffectController = unit.GetComponent<AttackEffectController>();
                    if (attackEffectController != null)
                    {
                        attackEffectController.StopParticleSystem();
                    }

                    // TODO destroy by effect
                    // TODO destroy Game Object
                    unit.GetComponent<AudioSource>().Stop();
                    break;
                }
                case UnitActionType.Teleport:
                {
                    Debug.Log("Teleport on " + unitAction.UnitId + " on turn " + _turnNumber);
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    var moveController = unit.GetComponent<MoveController>();
                    moveController.StopEveryThing();
                    unit.transform.position = new Vector3(unitAction.Col, unit.transform.position.y, unitAction.Row);
                    var attackEffectController = unit.GetComponent<AttackEffectController>();
                    if (attackEffectController != null)
                    {
                        attackEffectController.StopParticleSystem();
                    }

                    unit.GetComponent<AudioSource>().Stop();
                    break;
                }
            }

            _unitActionsPointer++;
        }
    }

    private void ApplySpellActions()
    {
        while (_spellActionsPointer < _spellActions.Count && _spellActions[_spellActionsPointer].Time <= _time)
        {
            var spellAction = _spellActions[_spellActionsPointer];
            if (spellAction.ActionType == SpellActionType.Put)
            {
                foreach (var sec in spellAction.UnitIds.Select(sid => _gameUnitFactory.FindById(sid))
                    .Select(unit => unit.GetComponent<SpellEffectController>()).Where(sec => sec != null))
                {
                    sec.StartSpell(spellAction.TypeId);
                }
            }
            else
            {
                foreach (var sec in spellAction.UnitIds.Select(sid => _gameUnitFactory.FindById(sid))
                    .Select(unit => unit.GetComponent<SpellEffectController>()).Where(sec => sec != null))
                {
                    sec.StopSpell(spellAction.TypeId);
                }
            }

            _spellActionsPointer++;
        }
    }

    private void FireUIEvents(List<GameTurn> gameTurns, int turnNumber)
    {
        //Debug.Log(turnNumber);
        if (turnNumber >= gameTurns.Count) return;
        var uiController = GetComponent<UIContoller>();
        uiController.UpdateTurnNumberBroadcast(turnNumber);
        var turn = gameTurns[turnNumber];
        uiController.FireUIEvents(turn);
    }
}