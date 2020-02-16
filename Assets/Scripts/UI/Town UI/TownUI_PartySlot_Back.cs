using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TownUI_PartySlot_Back : MonoBehaviour, IDropHandler
{
    [Header("Edit mode stuff")]
    public Image slotImg;

    [Header("Runtime stuff")]
    public InventorySlot invSlot;
    public string slotName;
    public TownUI_PartySlot_Front slotFront;

    private TownUI_Panel_Parties partiesPanel;

    void Awake()
    {
        slotFront = GetComponentInChildren<TownUI_PartySlot_Front>();
        partiesPanel = GetComponentInParent<TownUI_Panel_Parties>();
    }

    public void UpdateSlot(InventorySlot invSlot)
    {
        this.invSlot = invSlot;
        slotName = invSlot.slotName;

        Artifact artifact = invSlot.artifact;
        slotFront.ChangeImage(artifact?.dbData.image);
    }

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;
        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, InputManager.Instance.mouseScreenPos))
        {
            partiesPanel.InvSlotDrop(this);
        }
    }
}
