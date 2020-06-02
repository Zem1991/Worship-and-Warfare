using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallPiece3 : TownDefensePiece3
{
    [Header("UI components")]
    public RectTransform uiBarRect;
    public Image uiHealthBar;

    protected override void Update()
    {
        base.Update();

        bool showUI = !stateDead && ICP_IsIdle();
        uiBarRect.gameObject.SetActive(showUI);
        uiHealthBar.fillAmount = ((float)healthStats.hitPoints_current) / healthStats.hitPoints_maximum;
    }

    protected override void AP3_UpdateAnimatorParameters()
    {
        //TODO: something later?
    }
}
