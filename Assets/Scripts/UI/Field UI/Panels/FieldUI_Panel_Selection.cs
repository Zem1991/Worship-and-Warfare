using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_Selection : AbstractUIPanel, IPartyPieceRefresh
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

    public void UpdatePanel(TownPiece3 town)
    {
        HideInformations();
        if (!town) return;

        townInfo.RefreshInfo(town);
        townInfo.Show();
        txtSelectionTitle.text = town.AFP3_GetPieceTitle();
    }

    public void UpdatePanel(PartyPiece3 party)
    {
        HideInformations();
        if (!party) return;

        partyInfo.RefreshInfo(party);
        partyInfo.Show();
        txtSelectionTitle.text = party.AFP3_GetPieceTitle();
    }

    public void UpdatePanel(PickupPiece3 pickup)
    {
        HideInformations();
        if (!pickup) return;

        pickupInfo.RefreshInfo(pickup, txtSelectionTitle);
        pickupInfo.Show();
        txtSelectionTitle.text = pickup.AFP3_GetPieceTitle();
    }

    public void PartyPieceRefresh(PartyPiece3 partyPiece)
    {
        throw new System.NotImplementedException();
    }
}
