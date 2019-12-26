using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnitFactory
{
    private List<GameUnit> gameUnits = new List<GameUnit>();
    
    public GameObject FindById(int id)
    {
        foreach (GameUnit gameUnit in gameUnits)
            if (gameUnit.id == id)
                return gameUnit.unit;
        return null;
    }

    public void AddGameUnit(int id, GameObject gameObject)
    {
        gameUnits.Add(new GameUnit(gameObject, id));
    }
}
