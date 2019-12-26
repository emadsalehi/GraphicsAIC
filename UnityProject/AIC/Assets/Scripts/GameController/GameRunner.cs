using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    public float turnTime = 2.0f;
    public int unitNumbers = 9;
    public List<GameObject> playerGameObjects;
    
    private List<UnitAction> UnitActions;
    private List<SpellAction> SpellActions;
    private float time = 0.0f;
    private int unitActionsPointer = 0;
    private int spellActionsPointer = 0;
    private GameUnitFactory GameUnitFactory = new GameUnitFactory();
    // Start is called before the first frame update
    void Start()
    {
        Game game = gameObject.GetComponent<LogReader>().ReadLog();
        // TODO Call MakeMap in maker with log init
        LogParser logParser = gameObject.GetComponent<LogParser>();
        logParser.TurnTime = turnTime;
        logParser.ParseLog(game);
        UnitActions = logParser.UnitActions;
        SpellActions = logParser.SpellActions;
    }

    // Update is called once per frame
    void Update()
    {
        while (UnitActions[unitActionsPointer].Time <= time)
        {
            UnitAction unitAction = UnitActions[unitActionsPointer];
            if (unitAction.ActionType == UnitActionType.StartMove)
            {
                GameObject unit = GameUnitFactory.FindById(unitAction.UnitId);
                MoveController moveController = unit.GetComponent<MoveController>();
                AnimatorController animatorController = unit.GetComponent<AnimatorController>();
                moveController.turnTime = turnTime;
                animatorController.SetTurnTime(turnTime);
                animatorController.Restart();
                animatorController.StartMoving();
                moveController.StartMoving();
            } 
            else if (unitAction.ActionType == UnitActionType.MoveAfterRotate)
            {
                GameObject unit = GameUnitFactory.FindById(unitAction.UnitId);
                MoveController moveController = unit.GetComponent<MoveController>();
                AnimatorController animatorController = unit.GetComponent<AnimatorController>();
                moveController.turnTime = turnTime;
                animatorController.SetTurnTime(turnTime);
                animatorController.Restart();
                animatorController.MoveAfterRotate();
                moveController.StartMovingAfterRotate(new Vector3(unitAction.Col, 0, unitAction.Row));
            }
            else if (unitAction.ActionType == UnitActionType.Rotate)
            {
                GameObject unit = GameUnitFactory.FindById(unitAction.UnitId);
                if (unit == null)
                {
                    unit = Instantiate(playerGameObjects[unitAction.PId * unitNumbers + unitAction.Value]
                        , new Vector3(unitAction.Row, 0, unitAction.Col), Quaternion.identity);
                    GameUnitFactory.AddGameUnit(unitAction.UnitId, unit);
                    unit.GetComponent<AnimatorController>().Deploy();
                }
                MoveController moveController = unit.GetComponent<MoveController>();
                AnimatorController animatorController = unit.GetComponent<AnimatorController>();
                moveController.turnTime = turnTime;
                animatorController.SetTurnTime(turnTime);
                animatorController.Restart();
                animatorController.Rotate();
                moveController.StartRotating(unitAction.Value - unit.transform.rotation.y);
            }
            else if (unitAction.ActionType == UnitActionType.StopMove)
            {
                GameObject unit = GameUnitFactory.FindById(unitAction.UnitId);
                if (unit == null)
                {
                    unit = Instantiate(playerGameObjects[unitAction.PId * unitNumbers + unitAction.Value]
                        , new Vector3(unitAction.Row, 0, unitAction.Col), Quaternion.identity);
                    GameUnitFactory.AddGameUnit(unitAction.UnitId, unit);
                    unit.GetComponent<AnimatorController>().Deploy();
                }
                MoveController moveController = unit.GetComponent<MoveController>();
                AnimatorController animatorController = unit.GetComponent<AnimatorController>();
                moveController.turnTime = turnTime;
                animatorController.SetTurnTime(turnTime);
                animatorController.Restart();
                animatorController.StopMove();
                moveController.StopEveryThing();
                // TODO enable attack effect on unit
                // TODO rotate to defender unit and look at it
            }
            else if (unitAction.ActionType == UnitActionType.Die)
            {
                GameObject unit = GameUnitFactory.FindById(unitAction.UnitId);
                MoveController moveController = unit.GetComponent<MoveController>();
                AnimatorController animatorController = unit.GetComponent<AnimatorController>();
                moveController.turnTime = turnTime;
                animatorController.SetTurnTime(turnTime);
                animatorController.Restart();
                animatorController.Die();
                moveController.StopEveryThing();
                // TODO destroy by effect
                // TODO destroy Game Object
                // TODO Play Die sound
            }
            unitActionsPointer++;
        }
        while (SpellActions[spellActionsPointer].Time <= time)
        {
            SpellAction spellAction = SpellActions[spellActionsPointer];
            // TODO create GameSpellFactory
            if (spellAction.ActionType == SpellActionType.Pick)
            { 
                // TODO place spell and play it
            }
            else
            {
                // TODO remove spell and stop it
            }
            spellActionsPointer++;
        }
        time += Time.deltaTime;
    }

    public void ChangeTurnTime(float turnTime)
    {
        this.turnTime = turnTime;
        // TODO 
    }
}
