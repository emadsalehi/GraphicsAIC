using System.Collections;
using System.Collections.Generic;

public class UnitFactory
{
    public List<UnitDetails> unitDetailsList = new List<UnitDetails>();

    public void AddUnitDetail(int pId, PlayerUnit playerUnit, int turnNum)
    {
        bool found = false;
        foreach (UnitDetails unitDetails in unitDetailsList)
        {
            if (unitDetails.id == playerUnit.Id && unitDetails.pId == pId)
            {
                found = true;
                unitDetails.unitEvents.Add(new UnitEvent(playerUnit.Row, playerUnit.Col));
                break;
            }
        }

        if (!found)
        {
            UnitDetails unitDetails = new UnitDetails(pId, playerUnit.Id, turnNum, playerUnit.TypeId);
            unitDetails.unitEvents.Add(new UnitEvent(playerUnit.Row, playerUnit.Col));
            unitDetailsList.Add(unitDetails);
        }
    }
}
