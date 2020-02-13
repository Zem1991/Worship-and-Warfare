using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitsInfo : MonoBehaviour, IShowableHideable
{
    public Image u1Portrait;
    public Text u1StackSize;
    public Image u2Portrait;
    public Text u2StackSize;
    public Image u3Portrait;
    public Text u3StackSize;
    public Image u4Portrait;
    public Text u4StackSize;
    public Image u5Portrait;
    public Text u5StackSize;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void RefreshInfo(List<Unit> units)
    {
        u1Portrait.sprite = null;
        u1StackSize.text = "--";
        u2Portrait.sprite = null;
        u2StackSize.text = "--";
        u3Portrait.sprite = null;
        u3StackSize.text = "--";
        u4Portrait.sprite = null;
        u4StackSize.text = "--";
        u5Portrait.sprite = null;
        u5StackSize.text = "--";

        if (units == null) return;

        if (units.Count >= 1)
        {
            u1Portrait.sprite = units[0].dbData.profilePicture;
            u1StackSize.text = units[0].stackStats.stack_maximum.ToString();
        }
        if (units.Count >= 2)
        {
            u2Portrait.sprite = units[1].dbData.profilePicture;
            u2StackSize.text = units[1].stackStats.stack_maximum.ToString();
        }
        if (units.Count >= 3)
        {
            u3Portrait.sprite = units[2].dbData.profilePicture;
            u3StackSize.text = units[2].stackStats.stack_maximum.ToString();
        }
        if (units.Count >= 4)
        {
            u4Portrait.sprite = units[3].dbData.profilePicture;
            u4StackSize.text = units[3].stackStats.stack_maximum.ToString();
        }
        if (units.Count >= 5)
        {
            u5Portrait.sprite = units[4].dbData.profilePicture;
            u5StackSize.text = units[4].stackStats.stack_maximum.ToString();
        }
    }
}
