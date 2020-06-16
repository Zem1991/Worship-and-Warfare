using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUnitPiece3 : UnitPiece3
{
    [Header("UI components")]
    public RectTransform uiBarRect;
    public Image uiHealthBar;
    public RectTransform uiStackRect;
    public Text uiStackSizeText;

    //TODO stack size at battle start?

    protected override void Update()
    {
        base.Update();

        bool showUI = !stateDead && ICP_IsIdle();
        uiBarRect.gameObject.SetActive(showUI);
        uiHealthBar.fillAmount = ((float)healthStats.hitPoints_current) / healthStats.hitPoints_maximum;
        uiStackRect.gameObject.SetActive(showUI);
        uiStackSizeText.text = "" + GetStackHealthStats().GetStackSize();
    }

    public CombatUnit GetCombatUnit()
    {
        return abstractUnit as CombatUnit;
    }

    public StackHealthStats2 GetStackHealthStats()
    {
        return healthStats as StackHealthStats2;
    }
}
