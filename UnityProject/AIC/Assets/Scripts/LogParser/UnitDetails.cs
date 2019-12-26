using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDetails
{
    public int pId;
    public int id;
    public int startTurn;
    public int typeId;
    public List<UnitEvent> unitEvents = new List<UnitEvent>();

    public UnitDetails(int pId, int id, int startTurn, int typeId)
    {
        this.pId = pId;
        this.id = id;
        this.startTurn = startTurn;
        this.typeId = typeId;
    }
}
