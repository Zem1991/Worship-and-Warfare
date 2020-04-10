using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_Selection : AbstractUIPanel
{
    public Text txtSelectionTitle;

    public UI_TownInfo townInfo;
    public UI_PartyInfo partyInfo;
    public UI_PickupInfo pickupInfo;

    public void HideInformations()
    {
        txtSelectionTitle.text = "--";

        townInfo.Hide();
        partyInfo.Hide();
        pickupInfo.Hide();
    }

    public void UpdatePanel(TownPiece2 town)
    {
        HideInformations();
        if (!town) return;

        townInfo.RefreshInfo(town);
        townInfo.Show();
        txtSelectionTitle.text = town.AFP2_GetPieceTitle();
    }

    public void UpdatePanel(PartyPiece2 party)
    {
        HideInformations();
        if (!party) return;

        partyInfo.RefreshInfo(party.party);
        partyInfo.Show();
        txtSelectionTitle.text = party.AFP2_GetPieceTitle();
    }

    public void UpdatePanel(AbstractPickupPiece2 pickup)
    {
        HideInformations();
        if (!pickup) return;

        pickupInfo.RefreshInfo(pickup, txtSelectionTitle);
        pickupInfo.Show();
        txtSelectionTitle.text = pickup.AFP2_GetPieceTitle();
    }
}
