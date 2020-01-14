using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CombatActorInspector_MovementStats : MonoBehaviour
{
    public Text txtInitiative;
    public Text txtMovePoints;
    public Text txtMoveType;

    public void RefreshInfo(AbstractCombatActorPiece2 acap)
    {
        AbstractCombatantPiece2 combatant = acap as AbstractCombatantPiece2;

        txtInitiative.text = "" + acap.combatPieceStats.initiative;

        if (combatant)
        {
            txtMovePoints.gameObject.SetActive(true);
            txtMoveType.gameObject.SetActive(true);

            txtMovePoints.text = "" + combatant.combatPieceStats.movementRange;// + "/" + hero.combatPieceStats.hitPoints_maximum;
            txtMoveType.text = "" + combatant.combatPieceStats.movementType;
        }
        else
        {
            txtMovePoints.gameObject.SetActive(false);
            txtMoveType.gameObject.SetActive(false);
        }
    }
}
