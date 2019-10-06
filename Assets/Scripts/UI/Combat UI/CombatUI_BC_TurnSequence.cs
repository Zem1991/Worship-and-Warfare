﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI_BC_TurnSequence : AUIPanel
{
    [Header("Sequence Bar")]
    public RectTransform sequenceBar;

    [Header("Instances")]
    public List<CUI_TurnSequenceItem> tsItems = new List<CUI_TurnSequenceItem>();

    public void UpdatePanel()
    {

    }

    public void DestroyTurnSequence()
    {
        foreach (CUI_TurnSequenceItem item in tsItems) Destroy(item.gameObject);
        tsItems.Clear();
    }

    public void CreateTurnSequence(List<CombatUnitPiece> turnSequence)
    {
        DestroyTurnSequence();

        CUI_TurnSequenceItem prefab = AllPrefabs.Instance.cuiTurnSequenceItem;

        foreach (CombatUnitPiece forCUP in turnSequence)
        {
            CUI_TurnSequenceItem newCUI = Instantiate(prefab, sequenceBar.transform);
            newCUI.border.color = forCUP.owner.color;
            newCUI.portrait.sprite = forCUP.unit.dbData.profilePicture;
            newCUI.unit = forCUP;
            tsItems.Add(newCUI);
        }
    }

    public void RemoveFirstFromTurnSequence()
    {
        CUI_TurnSequenceItem cuiTSI = tsItems[0];
        tsItems.RemoveAt(0);
        Destroy(cuiTSI.gameObject);
    }
}
