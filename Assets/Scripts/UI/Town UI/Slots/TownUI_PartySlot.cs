using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI_PartySlot : AUI_DNDSlot
{
    [Header("Runtime references - Party")]
    public PartySlot partySlot;

    public void UpdateSlot(AUI_PanelDragAndDrop panelDND, PartySlot partySlot)
    {
        this.panelDND = panelDND;
        this.partySlot = partySlot;

        AbstractUnit partyEl = partySlot?.Get();
        ChangeImage(partyEl?.AU_GetProfileImage());
    }

    public override void RightClick()
    {
        Party party = partySlot.party;
        party.SplitHalfFast(partySlot);

        UI_PartyInfo partyInfo = panelDND as UI_PartyInfo;
        partyInfo.RefreshInfo();
    }
}
