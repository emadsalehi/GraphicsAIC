using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellFactory
{
    public List<SpellDetails> spellDetailsList = new List<SpellDetails>();

    public void AddSpellDetails (int PId , PlayerMapSpell playerMapSpell , int turnTime , int row , int col ,int id , int range)
    {
        bool found = false;
        foreach (SpellDetails spellDetails in spellDetailsList)
        {
            if (spellDetails.id == playerMapSpell.SpellId && spellDetails.pId == PId )
            {
                found = true;
                spellDetails.aliveTurns += 1;
                break;
            }
        }
        if (!found){
            SpellDetails spellDetails = new SpellDetails(PId , id ,range, row , col , turnTime);
            spellDetailsList.Add(spellDetails);
        }
        
    }
    
}
