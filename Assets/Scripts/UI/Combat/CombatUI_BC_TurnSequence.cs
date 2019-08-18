using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI_BC_TurnSequence : AUIPanel
{
    [Header("Prefab")]
    public TurnSequenceItem prefabTSItem;

    [Header("Sequence Bar")]
    public RectTransform sequenceBar;

    [Header("Instances")]
    public List<TurnSequenceItem> tsItems;

    public void UpdatePanel()
    {

    }
}
