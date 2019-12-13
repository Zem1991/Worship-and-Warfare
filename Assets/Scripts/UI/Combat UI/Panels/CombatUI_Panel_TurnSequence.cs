using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI_Panel_TurnSequence : AUIPanel
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

    public void CreateTurnSequence(List<AbstractCombatActorPiece2> turnSequence, List<AbstractCombatActorPiece2> waitSequence)
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

    private void ProcessSequence(CombatUI_TurnSequenceItem prefab, List<AbstractCombatActorPiece2> list)
    {
        foreach (AbstractCombatActorPiece2 forCutp in list)
        {
            CombatUI_TurnSequenceItem newCUI = Instantiate(prefab, sequenceBar.transform);
            newCUI.border.color = forCutp.pieceOwner.GetOwner().dbColor.mainColor;

            Sprite portrait = null;
            CombatantHeroPiece2 chp = forCutp as CombatantHeroPiece2;
            CombatantUnitPiece2 cup = forCutp as CombatantUnitPiece2;
            if (chp) portrait = chp.hero.dbData.profilePicture;
            if (cup) portrait = cup.unit.dbData.profilePicture;
            if (portrait) newCUI.portrait.sprite = portrait;

            newCUI.combatPiece = forCutp;
            tsItems.Add(newCUI);
        }
    }
}
