using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellFactory
{
    public List<SpellDetails> spellDetailsList = new List<SpellDetails>();

    public void AddSpellDetails(int pId, PlayerMapSpell playerMapSpell, int turnTime, int id)
    {
        var found = false;
        foreach (var spellDetails in spellDetailsList.Where(spellDetails =>
            spellDetails.id == playerMapSpell.SpellId && spellDetails.pId == pId))
        {
            found = true;
            spellDetails.aliveTurns += 1;
            break;
        }

        if (found) return;
        {
            var spellDetails =
                new SpellDetails(pId, id, playerMapSpell.UnitIds, playerMapSpell.TypeId, turnTime);
            spellDetailsList.Add(spellDetails);
        }
    }
}