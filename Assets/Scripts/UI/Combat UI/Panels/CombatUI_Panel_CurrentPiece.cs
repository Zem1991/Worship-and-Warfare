using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI_Panel_CurrentPiece : AbstractUIPanel
{
    public Image unitPortrait;
    public Button btnWait;
    public Button btnDefend;
    public Button btnAbility1;
    public Button btnAbility2;
    public Button btnAbility3;

    public void UpdatePanel(CombatantPiece3 actp, bool canCommandSelectedPiece)
    {
        if (actp == null) return;

        UnitPiece3 unitPiece = actp as UnitPiece3;
        Sprite sprite = unitPiece ? unitPiece.abstractUnit.AU_GetProfileImage() : null;

        unitPortrait.sprite = sprite;
        UpdateWait(actp, canCommandSelectedPiece);
        UpdateDefend(actp, canCommandSelectedPiece);
        UpdateAbility1(actp, canCommandSelectedPiece);
        UpdateAbility2(actp, canCommandSelectedPiece);
        UpdateAbility3(actp, canCommandSelectedPiece);
    }

    private void UpdateWait(CombatantPiece3 actp, bool canCommandSelectedPiece)
    {
        bool notInState = !actp.pieceCombatActions.stateWait;
        bool canDoIt = actp.pieceCombatActions.canWait;
        btnWait.interactable = canCommandSelectedPiece && notInState;
        btnWait.gameObject.SetActive(canDoIt);
    }

    private void UpdateDefend(CombatantPiece3 actp, bool canCommandSelectedPiece)
    {
        bool notInState = !actp.pieceCombatActions.stateDefend;
        bool canDoIt = actp.pieceCombatActions.canDefend;
        btnDefend.interactable = canCommandSelectedPiece && notInState;
        btnDefend.gameObject.SetActive(canDoIt);
    }

    private void UpdateAbility1(CombatantPiece3 actp, bool canCommandSelectedPiece)
    {
        DB_Ability ability = actp.abilityStats.ability1;
        bool canDoIt = actp.pieceCombatActions.canDefend;
        btnAbility1.interactable = canCommandSelectedPiece && ability;
        btnAbility1.gameObject.SetActive(canDoIt);
    }

    private void UpdateAbility2(CombatantPiece3 actp, bool canCommandSelectedPiece)
    {
        DB_Ability ability = actp.abilityStats.ability2;
        bool canDoIt = actp.pieceCombatActions.canDefend;
        btnAbility2.interactable = canCommandSelectedPiece && ability;
        btnAbility2.gameObject.SetActive(canDoIt);
    }

    private void UpdateAbility3(CombatantPiece3 actp, bool canCommandSelectedPiece)
    {
        DB_Ability ability = actp.abilityStats.ability3;
        bool canDoIt = actp.pieceCombatActions.canDefend;
        btnAbility3.interactable = canCommandSelectedPiece && ability;
        btnAbility3.gameObject.SetActive(canDoIt);
    }
}
