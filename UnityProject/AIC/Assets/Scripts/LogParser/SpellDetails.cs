using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDetails
{
    public int pId;
    public int id;
    public int startTurn;
    public int aliveTurns = 0;
    public int typeId;
    public List<int> unitIds;


    public SpellDetails(int pId ,int id ,List<int> unitIds , int typeId,int startTurn)
    {
        this.pId = pId;
        this.id = id;
        this.unitIds = unitIds;
        this.typeId = typeId;
        this.startTurn = startTurn;
    }
}
