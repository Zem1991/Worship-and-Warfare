using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AES_Action_Damage : AES_Action
{
    public override bool Execute(AbstractPartyElement actionUser, List<AbstractTile> targetArea)
    {
        //TODO do something with the actionUser

        foreach (AbstractTile tile in targetArea)
        {
            AbstractCombatActorPiece2 combatActor = tile.occupantPiece as AbstractCombatActorPiece2;
            if (combatActor)
            {
                int amount = parameters.paramFormula.EvaluateFormula();
                combatActor.ReceiveDamage(amount);
            }
        }

        return true;
    }
}
