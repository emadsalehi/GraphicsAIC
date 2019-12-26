using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDetails
{
    public int pId;
    public int id;
    public int range;
    public int row; 
    public int col;
    public int startTurn;
    public int aliveTurns = 0; 
    
    public SpellDetails(int pId ,int id ,int range ,int row ,int col ,int startTurn)
    {
        this.pId = pId;
        this.id = id;
        this.range = range;
        this.row = row;
        this.col = col;
        this.startTurn = startTurn;
    }
}
