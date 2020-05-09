using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TownUI_Panel_RecruitCreature_CreatureOption : MonoBehaviour
{
    [Header("Static reference")]
    public Text txtUnitName;
    public Image unitImage;
    public Text txtAvailable;
    public InputField inpAmount;

    [Header("Dynamic reference")]
    public TownUI_Panel_RecruitCreature parentPanel;
    public DB_CombatUnit dbUnit;
    public int amount;

    public void CheckAmountRange()
    {
        bool parsed = int.TryParse(inpAmount.text, out int amount);
        if (parsed) amount = Mathf.Clamp(amount, 0, 9999);
        else amount = 0;
        inpAmount.text = "" + amount;
        this.amount = amount;
        parentPanel.CheckAmounts();
    }
}
