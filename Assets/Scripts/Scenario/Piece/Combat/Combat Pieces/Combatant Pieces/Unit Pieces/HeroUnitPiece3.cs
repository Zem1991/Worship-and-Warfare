using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUnitPiece3 : UnitPiece3
{
    [Header("UI components")]
    public RectTransform uiBarRect;
    public Image uiHealthBar;
    public Image uiManaBar;

    protected override void Update()
    {
        base.Update();

        bool showUI = !stateDead && ICP_IsIdle();
        uiBarRect.gameObject.SetActive(showUI);
        uiHealthBar.fillAmount = ((float)healthStats.hitPoints_current) / healthStats.hitPoints_maximum;
        uiManaBar.fillAmount = 0.8F;    //TODO: MANA POINTS!
    }

    public HeroUnit GetHeroUnit()
    {
        return abstractUnit as HeroUnit;
    }
}
