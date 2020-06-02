using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI_Panel_TurnSequence : AbstractUIPanel
{
    [Header("Sequence Bar")]
    public RectTransform sequenceBar;

    [Header("Instances")]
    public List<CombatUI_TurnSequenceItem> tsItems = new List<CombatUI_TurnSequenceItem>();

    public void UpdatePanel()
    {

    }

    public void DestroyTurnSequence()
    {
        foreach (CombatUI_TurnSequenceItem item in tsItems) Destroy(item.gameObject);
        tsItems.Clear();
    }

    public void CreateTurnSequence(List<CombatantPiece3> turnSequence, List<CombatantPiece3> waitSequence)
    {
        DestroyTurnSequence();

        CombatUI_TurnSequenceItem prefab = AllPrefabs.Instance.cuiTurnSequenceItem;
        ProcessSequence(prefab, turnSequence);
        ProcessSequence(prefab, waitSequence);
    }

    public void RemoveFirstFromTurnSequence()
    {
        CombatUI_TurnSequenceItem cuiTSI = tsItems[0];
        tsItems.RemoveAt(0);
        Destroy(cuiTSI.gameObject);
    }

    private void ProcessSequence(CombatUI_TurnSequenceItem prefab, List<CombatantPiece3> list)
    {
        foreach (CombatantPiece3 forCutp in list)
        {
            CombatUI_TurnSequenceItem newCUI = Instantiate(prefab, sequenceBar.transform);
            newCUI.border.color = forCutp.pieceOwner.Get().dbColor.mainColor;

            Sprite portrait = null;
            HeroUnitPiece3 chp = forCutp as HeroUnitPiece3;
            CombatUnitPiece3 cup = forCutp as CombatUnitPiece3;
            if (chp) portrait = chp.GetHeroUnit().AU_GetProfileImage();
            if (cup) portrait = cup.GetCombatUnit().AU_GetProfileImage();
            if (portrait) newCUI.portrait.sprite = portrait;

            newCUI.combatPiece = forCutp;
            tsItems.Add(newCUI);
        }
    }
}
