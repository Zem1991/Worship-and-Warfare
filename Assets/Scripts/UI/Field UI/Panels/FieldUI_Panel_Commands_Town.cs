using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_Commands_Town : AbstractUIPanel
{
    public Button btnEnterTown;
    public Button btnEjectGarrison;

    public void HideButtons()
    {
        btnEnterTown.gameObject.SetActive(false);
        btnEjectGarrison.gameObject.SetActive(false);
    }

    public void ShowButtons()
    {
        btnEnterTown.gameObject.SetActive(true);
        btnEjectGarrison.gameObject.SetActive(true);
    }

    public void UpdatePanel()
    {
        HideButtons();
    }

    public void UpdatePanel(TownPiece3 tp)
    {
        UpdatePanel();
        if (!tp) return;

        ShowButtons();

        if (tp.IPFC_GetPartyForCombat().GetMostRelevant())
        {
            btnEjectGarrison.interactable = true;
        }
        else
        {
            btnEjectGarrison.interactable = false;
        }
    }
}
