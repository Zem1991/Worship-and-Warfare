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

    public void UpdatePanel(AbstractCombatantPiece2 actp, bool canCommandSelectedPiece)
    {
        if (actp == null) return;

        unitPortrait.sprite = actp.GetPartyElement().GetProfileImage();
        UpdateWait(actp, canCommandSelectedPiece);
        UpdateDefend(actp, canCommandSelectedPiece);
        UpdateAbility1(actp, canCommandSelectedPiece);
        UpdateAbility2(actp, canCommandSelectedPiece);
        UpdateAbility3(actp, canCommandSelectedPiece);
    }

    private void UpdateWait(AbstractCombatantPiece2 actp, bool canCommandSelectedPiece)
    {
        bool notInState = !actp.pieceCombatActions.stateWait;
        bool canDoIt = actp.pieceCombatActions.canWait;
        btnWait.interactable = canCommandSelectedPiece && notInState;
        btnWait.gameObject.SetActive(canDoIt);
    }

    private void UpdateDefend(AbstractCombatantPiece2 actp, bool canCommandSelectedPiece)
    {
        bool notInState = !actp.pieceCombatActions.stateDefend;
        bool canDoIt = actp.pieceCombatActions.canDefend;
        btnDefend.interactable = canCommandSelectedPiece && notInState;
        btnDefend.gameObject.SetActive(canDoIt);
    }

    private void UpdateAbility1(AbstractCombatantPiece2 actp, bool canCommandSelectedPiece)
    {
        DB_Ability ability = actp.combatPieceStats.ability1;
        bool canDoIt = actp.pieceCombatActions.canDefend;
        btnAbility1.interactable = canCommandSelectedPiece && ability;
        btnAbility1.gameObject.SetActive(canDoIt);
    }

    private void UpdateAbility2(AbstractCombatantPiece2 actp, bool canCommandSelectedPiece)
    {
        DB_Ability ability = actp.combatPieceStats.ability2;
        bool canDoIt = actp.pieceCombatActions.canDefend;
        btnAbility2.interactable = canCommandSelectedPiece && ability;
        btnAbility2.gameObject.SetActive(canDoIt);
    }

    private void UpdateAbility3(AbstractCombatantPiece2 actp, bool canCommandSelectedPiece)
    {
        DB_Ability ability = actp.combatPieceStats.ability3;
        bool canDoIt = actp.pieceCombatActions.canDefend;
        btnAbility3.interactable = canCommandSelectedPiece && ability;
        btnAbility3.gameObject.SetActive(canDoIt);
    }
}
