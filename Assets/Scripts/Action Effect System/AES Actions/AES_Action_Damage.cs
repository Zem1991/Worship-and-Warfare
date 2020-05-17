using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AES_Action_Damage : AES_Action
{
    public override bool Execute(AbstractUnit actionUser, List<AbstractTile> targetArea)
    {
        //TODO do something with the actionUser

        foreach (AbstractTile tile in targetArea)
        {
            CombatantPiece3 combatActor = tile.occupantPiece as CombatantPiece3;
            if (combatActor)
            {
                int amount = parameters.paramFormula.EvaluateFormula();
                combatActor.ReceiveDamage(amount);
            }
        }

        return true;
    }
}
