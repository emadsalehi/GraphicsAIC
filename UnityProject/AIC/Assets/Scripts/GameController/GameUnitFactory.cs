using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameUnitFactory
{
    private List<GameUnit> _gameUnits = new List<GameUnit>();
    
    public GameObject FindById(int id)
    {
        return (from gameUnit in _gameUnits where gameUnit.id == id select gameUnit.unit).FirstOrDefault();
    }

    public void AddGameUnit(int id, GameObject gameObject)
    {
        _gameUnits.Add(new GameUnit(gameObject, id));
    }

    public List<GameObject> GetAllUnits()
    {
        return _gameUnits.Select(gameUnit => gameUnit.unit).ToList();
    }

    public void RemoveUnit(int id)
    {
        foreach (var gameUnit in _gameUnits)
        {
            if (gameUnit.id == id)
            {
                _gameUnits.Remove(gameUnit);
                return;
            }
        }
    }
}
