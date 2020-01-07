using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDetails
{
    public int pId;
    public int id;
    public List<int> unitIds;
    public int startTurn;
    public int aliveTurns = 0; 
    
    public SpellDetails(int pId ,int id ,List<int> unitIds ,int startTurn)
    {
        this.pId = pId;
        this.id = id;
        this.unitIds = unitIds;
        this.startTurn = startTurn;
    }
}
