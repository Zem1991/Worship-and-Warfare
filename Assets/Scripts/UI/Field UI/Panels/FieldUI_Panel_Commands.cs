using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_Commands : AUIPanel
{
    public Image crest;

    public Button btnMovement;
    public Button btnDeployment;
    public Button btnSplitParty;
    public Button btnInventory;
    public Button btnHeroInfo;
    public Button btnSpellBook;

    public RectTransform rectMovePoints;
    public RectTransform rectManaPoints;

    public Text txtMovePoints;
    public Text txtManaPoints;

    public void HideButtons()
    {
        btnMovement.gameObject.SetActive(false);
        btnDeployment.gameObject.SetActive(false);
        btnSplitParty.gameObject.SetActive(false);
        btnHeroInfo.gameObject.SetActive(false);
        btnInventory.gameObject.SetActive(false);
        btnSpellBook.gameObject.SetActive(false);

        rectMovePoints.gameObject.SetActive(false);
        rectManaPoints.gameObject.SetActive(false);
    }

    public void ShowButtons()
    {
        btnMovement.gameObject.SetActive(true);
        btnDeployment.gameObject.SetActive(true);
        btnSplitParty.gameObject.SetActive(true);
        btnHeroInfo.gameObject.SetActive(true);
        btnInventory.gameObject.SetActive(true);
        btnSpellBook.gameObject.SetActive(true);

        rectMovePoints.gameObject.SetActive(true);
        rectManaPoints.gameObject.SetActive(true);
    }

    public void UpdatePanel()
    {
        HideButtons();

        txtMovePoints.text = "--";
        txtManaPoints.text = "--";
    }

    public void UpdatePanel(PartyPiece2 pp)
    {
        ShowButtons();

        PieceMovement pm = pp.IMP_GetPieceMovement();
        txtMovePoints.text = pm.movementPointsCurrent + "/" + pm.movementPointsMax;
        //txtManaPoints.text = pm.movementPointsCurrent + "/" + pm.movementPointsMax;   //TODO MANA

        if (pp.partyHero != null)
        {
            btnHeroInfo.interactable = true;
            btnSpellBook.interactable = true;
        }
        else
        {
            btnHeroInfo.interactable = false;
            btnSpellBook.interactable = false;
        }
    }
}
