using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityTemplateProjects;
using Random = UnityEngine.Random;

public class GameRunner : MonoBehaviour
{
    public float turnTime = 1.0f;
    [FormerlySerializedAs("baseTurnTile")] public float baseTurnTime = 1.0f;
    public int unitNumbers = 9;
    public List<GameObject> playerGameObjects;
    public Slider timeSlider;

    private List<UnitAction> _unitActions;
    private List<SpellAction> _spellActions;
    private List<KingAction> _kingActions;
    private List<GameTurn> _gameTurns;
    private GameInit _init;
    private List<EndGame> _end;
    private readonly List<GameObject> _towers = new List<GameObject>();
    private GameUnitFactory _gameUnitFactory;
    private LogParser _logParser;
    private AudioManager _audioManager;
    private const float TileSize = 1.0f;
    private const float FastForwardTurnTime = 0.09f;
    private float _time;
    private float _uiTime;
    private float _timeSpeed = 1.0f;
    private int _turnNumber;
    private int _unitActionsPointer;
    private int _spellActionsPointer;
    private int _kingActionPointer;
    private bool _isPaused;

    // Start is called before the first frame update
    void Start()
    {
        var game = gameObject.GetComponent<LogReader>().ReadLog();
        _gameTurns = game.turns;
        _init = game.init;
        _end = game.end;
        GetComponent<MapRenderer>().RenderMap(game.init, "FirstTile");
        GetComponent<MapEnvRenderer>().RenderMapEnv(_init.graphicMap.col, _init.graphicMap.row);
        _logParser = gameObject.GetComponent<LogParser>();
        _logParser.TurnTime = turnTime;
        _logParser.ParseLog(game);
        _unitActions = _logParser.UnitActions;
        _spellActions = _logParser.SpellActions;
        _kingActions = _logParser.KingActions;
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
        GetComponent<UIContoller>().canvas.BroadcastMessage("SetPlayers", game.init.graphicMap.kings);
        var cameras = GetComponent<CameraChanger>().cameras;
        cameras[0].transform.position = new Vector3(_init.graphicMap.col, 2.7f, -0.3f);
        foreach (var simpleCameraController in cameras.Select(camera => camera.GetComponent<SimpleCameraController>()))
        {
            simpleCameraController.xBounds = new[] {-0.5f, 0.5f + _init.graphicMap.col};
            simpleCameraController.yBounds = new[] {-0.5f, 0.5f + _init.graphicMap.row};
            simpleCameraController.zBounds = new[] {0.0f, 15.0f};
        }
    }

    // Update is called once per frame
    void Update()
    {
        ApplySpellActions();
        ApplyKingActions();
        ApplyUnitActions();
        _time += Time.deltaTime * _timeSpeed;
        _uiTime += Time.deltaTime;
        
        var newTurn = (int) Math.Truncate(_uiTime / turnTime);
        if (newTurn == _turnNumber) return;
        _turnNumber = newTurn;
        FireUiEvents(_gameTurns, _turnNumber);
    }

    public void ChangeTurnTime(float targetTurnTime)
    {
        _timeSpeed *= (turnTime / targetTurnTime);
        _uiTime /= (turnTime / targetTurnTime);
        turnTime = targetTurnTime;
        var units = _gameUnitFactory.GetAllUnits();
        foreach (var unit in units)
        {
            unit.GetComponent<MoveController>().turnTime = targetTurnTime;
            unit.GetComponent<AnimatorController>().SetTurnTime(targetTurnTime);
            unit.GetComponent<AudioSource>().pitch = _timeSpeed;
        }
    }

    public void ChangeGameTime()
    {
        var targetValue = timeSlider.value;
        var targetTurnValue = targetValue * (_gameTurns.Count - 1);
        var targetTime = targetTurnValue * baseTurnTime;
        if (targetTime < _time)
            return;
        StartCoroutine(SetNormalSpeed(turnTime, (targetTime - _time) * FastForwardTurnTime));
        ChangeTurnTime(FastForwardTurnTime);
    }

    IEnumerator SetNormalSpeed(float targetTurnTime, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeTurnTime(targetTurnTime);
    }

    public void PlayPauseGameRunner()
    {
        Time.timeScale = _isPaused ? 1.0f : 0.0f;
        _isPaused = !_isPaused;
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
                    moveController.speed = unitAction.Value;
                    moveController.StartMoving();
                    Debug.Log("StartMove on " + unitAction.UnitId + " on turn " + _turnNumber);
                    break;
                }
                case UnitActionType.MoveAfterRotate:
                {
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    var moveController = unit.GetComponent<MoveController>();
                    moveController.speed = unitAction.Value;
                    moveController.StartMovingAfterRotate(new Vector3(unitAction.Col, 0, unitAction.Row));
                    Debug.Log("MAFRotate on " + unitAction.UnitId + " on turn " + _turnNumber);
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
                    Debug.Log("Rotate on " + unitAction.UnitId + " on turn " + _turnNumber);
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
                    var attackEffectController = unit.GetComponent<AttackEffectController>();
                    Debug.Log("Attack from " + unitAction.UnitId + " on unit " + unitAction.TargetUnitId + " on turn " +
                              _turnNumber);
                    if (attackEffectController != null)
                    {
                        attackEffectController.PlayParticleSystem(target);
                    }
                    unit.GetComponent<AudioSource>().Play();
                    break;
                }
                case UnitActionType.Die:
                {
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    if (unit == null)
                    {
                        Debug.Log("Deploy3 on " + unitAction.UnitId + " on turn " + _turnNumber);
                        var xOffset = Random.Range(-TileSize / 3, TileSize / 3);
                        var zOffset = Random.Range(-TileSize / 3, TileSize / 3);
                        unit = Instantiate(playerGameObjects[unitAction.PId * unitNumbers + unitAction.TypeId]
                            , new Vector3(unitAction.Col + xOffset, playerGameObjects[unitAction.PId * unitNumbers + unitAction.TypeId].transform.position.y, unitAction.Row + zOffset),
                            Quaternion.identity);
                        _gameUnitFactory.AddGameUnit(unitAction.UnitId, unit);
                        unit.GetComponent<AnimatorController>().DeployDie();
                        var moveController1 = unit.GetComponent<MoveController>();
                        var animatorController1 = unit.GetComponent<AnimatorController>();
                        moveController1.turnTime = turnTime;
                        animatorController1.SetTurnTime(turnTime);
                    }
                    Debug.Log("Die on " + unitAction.UnitId + " on turn " + _turnNumber);
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
                    unit.GetComponent<AudioSource>().Stop();
                    break;
                }
                case UnitActionType.Destroy:
                {
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    _gameUnitFactory.RemoveUnit(unitAction.UnitId);
                    Destroy(unit);
                    Debug.Log("Destroy on " + unitAction.UnitId + " on turn " + _turnNumber);
                    break;
                }
                case UnitActionType.Teleport:
                {
                    Debug.Log("Teleport on " + unitAction.UnitId + " on turn " + _turnNumber);
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    var animatorController = unit.GetComponent<AnimatorController>();
                    animatorController.Restart();
                    var spellEffectController = unit.GetComponent<SpellEffectController>();
                    spellEffectController.StartSpell(3);
                    var moveController = unit.GetComponent<MoveController>();
                    moveController.StopEveryThing();
                    unit.transform.position = new Vector3(unitAction.Col, unit.transform.position.y, unitAction.Row);
                    var attackEffectController = unit.GetComponent<AttackEffectController>();
                    if (attackEffectController != null)
                    {
                        attackEffectController.StopParticleSystem();
                    }
                    Debug.Log("Teleport on " + unitAction.UnitId + " on turn " + _turnNumber);
                    unit.GetComponent<AudioSource>().Stop();
                    break;
                }
                case UnitActionType.Haste:
                {
                    Debug.Log("Haste on " + unitAction.UnitId + " on turn " + _turnNumber);
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    var animatorController = unit.GetComponent<AnimatorController>();
                    animatorController.Restart();
                    animatorController.StartMoving();
                    var spellEffectController = unit.GetComponent<SpellEffectController>();
                    spellEffectController.StartSpell(0);
                    var moveController = unit.GetComponent<MoveController>();
                    unit.transform.Rotate(0.0f, (float) Math.Atan2(unitAction.Col, unitAction.Row), 0.0f);
                    moveController.SetDirection(new Vector3(unitAction.Col, 0.0f, unitAction.Row));
                    moveController.StopEveryThing();
                    moveController.StartMoving();
                    moveController.speed = unitAction.Value;
                    Debug.Log("Haste on " + unitAction.UnitId + " on turn " + _turnNumber);
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
                    .Where(unit => unit != null)
                    .Select(unit => unit.GetComponent<SpellEffectController>()).Where(sec => sec != null))
                {
                    sec.StartSpell(spellAction.TypeId);
                }
            }
            else
            {
                foreach (var sec in spellAction.UnitIds.Select(sid => _gameUnitFactory.FindById(sid))
                    .Where(unit => unit != null)
                    .Select(unit => unit.GetComponent<SpellEffectController>()).Where(sec => sec != null))
                {
                    sec.StopSpell(spellAction.TypeId);
                }
            }
            _spellActionsPointer++;
        }
    }
    
    private void ApplyKingActions()
    {
        while (_kingActionPointer < _kingActions.Count && _kingActions[_kingActionPointer].Time <= _time)
        {
            var kingAction = _kingActions[_kingActionPointer];
            switch (kingAction.KingActionType)
            {
                case KingActionType.Attack:
                {
                    var unit = _gameUnitFactory.FindById(kingAction.TargetUnitId);
                    var attackEffectController =
                        _towers.ElementAt(kingAction.PId).GetComponent<AttackEffectController>();
                    if (attackEffectController != null)
                    {
                        attackEffectController.PlayParticleSystem(unit);
                    }
                    break;
                }
                case KingActionType.ChangeTarget:
                {
                    var unit = _gameUnitFactory.FindById(kingAction.TargetUnitId);
                    var attackEffectController =
                        _towers.ElementAt(kingAction.PId).GetComponent<AttackEffectController>();
                    if (attackEffectController != null)
                    {
                        attackEffectController.PlayParticleSystem(unit);
                    }

                    break;
                }
                case KingActionType.StopAttack:
                {
                    var attackEffectController =
                        _towers.ElementAt(kingAction.PId).GetComponent<AttackEffectController>();
                    if (attackEffectController != null)
                    {
                        attackEffectController.StopParticleSystem();
                    }
                    break;
                }
                case KingActionType.Die:
                {
                    var attackEffectController =
                        _towers.ElementAt(kingAction.PId).GetComponent<AttackEffectController>();
                    if (attackEffectController != null)
                    {
                        attackEffectController.StopParticleSystem();
                    }
                    var explosion = _towers.ElementAt(kingAction.PId).GetComponent<ExplosionManager>();
                    explosion.PlayExplosion();
                    StartCoroutine(RemoveTower(kingAction.PId, 1.5f * turnTime));
                    break;
                }
            }
            _kingActionPointer++;
        }
    }

    private IEnumerator RemoveTower(int targetTowerId, float delay)
    {
        yield return new WaitForSeconds(delay);
        _towers.ElementAt(targetTowerId).SetActive(false);
    }

    private void FireUiEvents(IReadOnlyList<GameTurn> gameTurns, int turnNumber)
    {
        var uiController = GetComponent<UIContoller>();
        if (turnNumber > gameTurns.Count) return;
        if (turnNumber == gameTurns.Count)
        {
            uiController.FireEndGameEvents(_end, _init.graphicMap.kings);
            return;
        }
        uiController.canvas.BroadcastMessage("SetPlayers", _init.graphicMap.kings);
        uiController.canvas.BroadcastMessage("SetMaxAP", _init.maxAP);
        uiController.UpdateTurnNumberBroadcast(turnNumber);
        uiController.UpdateSlider(turnNumber, _gameTurns.Count - 1);
        var turn = gameTurns[turnNumber];
        uiController.FireUiEvents(turn);
    }
    
    public void BackToMenu()
    {
        if (GameObject.Find("SoundController") != null)
        {
            _audioManager = GameObject.Find("SoundController").GetComponent<AudioManager>();
            _audioManager.Stop("Game");
            _audioManager.Play("Menu");
        }
        SceneManager.LoadScene(0);
    }
}