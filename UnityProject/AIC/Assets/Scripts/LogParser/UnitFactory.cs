using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UnitFactory
{
    public List<UnitDetails> unitDetailsList = new List<UnitDetails>();

    public void AddUnitDetail(int pId, PlayerUnit playerUnit, int turnNum)
    {
        var found = false;
        foreach (var unitDetails in unitDetailsList.Where(unitDetails => unitDetails.id == playerUnit.id && unitDetails.pId == pId))
        {
            found = true;
            unitDetails.unitEvents.Add(new UnitEvent(playerUnit.row, playerUnit.col));
            break;
        }

        if (found) return;
        {
            var unitDetails = new UnitDetails(pId, playerUnit.id, turnNum, playerUnit.typeId);
            unitDetails.unitEvents.Add(new UnitEvent(playerUnit.row, playerUnit.col));
            unitDetailsList.Add(unitDetails);
        }
    }
}
