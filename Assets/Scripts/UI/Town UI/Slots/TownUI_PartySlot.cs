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

        AbstractPartyElement partyEl = partySlot?.GetSlotObject();
        ChangeImage(partyEl?.GetProfileImage());
    }
}
