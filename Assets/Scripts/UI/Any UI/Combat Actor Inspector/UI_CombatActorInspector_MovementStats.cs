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

    public void RefreshInfo(CombatantPiece3 acap)
    {
        txtInitiative.text = "" + acap.movementStats.initiative;

        UnitPiece3 combatant = acap as UnitPiece3;
        if (combatant)
        {
            txtMovePoints.gameObject.SetActive(true);
            txtMoveType.gameObject.SetActive(true);

            txtMovePoints.text = "" + combatant.movementStats.movementRange;// + "/" + hero.combatPieceStats.hitPoints_maximum;
            txtMoveType.text = "" + combatant.movementStats.movementType;
        }
        else
        {
            txtMovePoints.gameObject.SetActive(false);
            txtMoveType.gameObject.SetActive(false);
        }
    }
}
