using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit
{
    public GameObject unit;
    public int id;

    public GameUnit(GameObject unit, int id)
    {
        this.unit = unit;
        this.id = id;
    }
}
