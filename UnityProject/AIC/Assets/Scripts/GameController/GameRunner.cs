using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    public float turnTime = 2.0f;
    public int unitNumbers = 9;
    public List<GameObject> playerGameObjects;
    
    private List<UnitAction> _unitActions;
    private List<SpellAction> _spellActions;
    private float _time = 0.0f;
    private int _unitActionsPointer = 0;
    private int _spellActionsPointer = 0;
    private LogParser _logParser;
    private GameUnitFactory _gameUnitFactory;
    
    // Start is called before the first frame update
    void Start()
    {
        var game = gameObject.GetComponent<LogReader>().ReadLog();
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
        while (_unitActions[_unitActionsPointer].Time <= _time)
        {
            var unitAction = _unitActions[_unitActionsPointer];
            switch (unitAction.ActionType)
            {
                case UnitActionType.StartMove:
                {
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    var moveController = unit.GetComponent<MoveController>();
                    var animatorController = unit.GetComponent<AnimatorController>();
                    moveController.turnTime = turnTime;
                    animatorController.SetTurnTime(turnTime);
                    animatorController.Restart();
                    animatorController.StartMoving();
                    moveController.StartMoving();
                    break;
                }
                case UnitActionType.MoveAfterRotate:
                {
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
                        unit = Instantiate(playerGameObjects[unitAction.PId * unitNumbers + unitAction.Value]
                            , new Vector3(unitAction.Row, 0, unitAction.Col), Quaternion.identity);
                        _gameUnitFactory.AddGameUnit(unitAction.UnitId, unit);
                        unit.GetComponent<AnimatorController>().Deploy();
                    }
                    var moveController = unit.GetComponent<MoveController>();
                    var animatorController = unit.GetComponent<AnimatorController>();
                    moveController.turnTime = turnTime;
                    animatorController.SetTurnTime(turnTime);
                    animatorController.Restart();
                    animatorController.Rotate();
                    moveController.StartRotating(unitAction.Value - unit.transform.rotation.y);
                    break;
                }
                case UnitActionType.StopMove:
                {
                    var unit = _gameUnitFactory.FindById(unitAction.UnitId);
                    if (unit == null)
                    {
                        unit = Instantiate(playerGameObjects[unitAction.PId * unitNumbers + unitAction.Value]
                            , new Vector3(unitAction.Row, 0, unitAction.Col), Quaternion.identity);
                        _gameUnitFactory.AddGameUnit(unitAction.UnitId, unit);
                        unit.GetComponent<AnimatorController>().Deploy();
                    }
                    var moveController = unit.GetComponent<MoveController>();
                    var animatorController = unit.GetComponent<AnimatorController>();
                    moveController.turnTime = turnTime;
                    animatorController.SetTurnTime(turnTime);
                    animatorController.Restart();
                    animatorController.StopMove();
                    moveController.StopEveryThing();
                    // TODO enable attack effect on unit
                    // TODO rotate to defender unit and look at it
                    break;
                }
                case UnitActionType.Die:
                {
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
        while (_spellActions[_spellActionsPointer].Time <= _time)
        {
            var spellAction = _spellActions[_spellActionsPointer];
            // TODO create GameSpellFactory
            if (spellAction.ActionType == SpellActionType.Pick)
            { 
                // TODO place spell and play it
            }
            else
            {
                // TODO remove spell and stop it
            }
            _spellActionsPointer++;
        }
        _time += Time.deltaTime;
    }

    public void ChangeTurnTime(float turnTime)
    {
        this.turnTime = turnTime;
        // TODO change turn time of all components
    }
}
