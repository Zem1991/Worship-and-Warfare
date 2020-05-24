using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AES_Action_Heal : AES_Action
{
    public override IEnumerator Execute(CombatantPiece3 actionUser, List<AbstractTile> targetArea)
    {
        //TODO do something with the actionUser

        List<IEnumerator> ienumerators = new List<IEnumerator>();
        foreach (AbstractTile tile in targetArea)
        {
            CombatantPiece3 combatActor = tile.occupantPiece as CombatantPiece3;
            if (combatActor)
            {
                int amount = parameters.paramFormula.EvaluateFormula();
                ienumerators.Add(combatActor.ReceiveHealing(amount));
            }
        }

        yield return ienumerators.Select(StartCoroutine).ToArray().GetEnumerator();
    }
}
