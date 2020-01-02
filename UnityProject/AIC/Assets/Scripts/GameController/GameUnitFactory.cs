using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnitFactory
{
    private List<GameUnit> _gameUnits = new List<GameUnit>();
    
    public GameObject FindById(int id)
    {
        foreach (var gameUnit in _gameUnits)
            if (gameUnit.id == id)
                return gameUnit.unit;
        return null;
    }

    public void AddGameUnit(int id, GameObject gameObject)
    {
        _gameUnits.Add(new GameUnit(gameObject, id));
    }
}
