using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI_BC_TurnSequence : AUIPanel
{
    [Header("Sequence Bar")]
    public RectTransform sequenceBar;

    [Header("Instances")]
    public List<CUI_TurnSequenceItem> tsItems;

    public void UpdatePanel()
    {

    }
}
