using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatantUnitPiece2 : AbstractCombatantPiece2
{
    [Header("UI references")]
    public RectTransform uiBarRect;
    public Image uiHealthBar;
    public RectTransform uiStackRect;
    public Text uiStackSizeText;

    [Header("Prefab references")]
    public StackStats stackStats;

    protected override void Update()
    {
        base.Update();

        bool showUI = !stateDead && ICP_IsIdle();
        uiBarRect.gameObject.SetActive(showUI);
        uiHealthBar.fillAmount = ((float)combatPieceStats.hitPoints_current) / combatPieceStats.hitPoints_maximum;
        uiStackRect.gameObject.SetActive(showUI);
        uiStackSizeText.text = "" + stackStats.Get();
    }

    public void Initialize(Unit unit, Player owner, int spawnId, bool defenderSide)
    {
        Initialize(owner, unit.combatPieceStats, spawnId, defenderSide);

        partyElement = unit;
        name = "P" + owner.id + " - Stack of " + unit.GetName();

        stackStats.Initialize(unit.stackStats.Get());

        IMP_ResetMovementPoints();
        SetAnimatorOverrideController(unit.dbData.animatorCombat);
    }

    public Unit GetUnit()
    {
        return partyElement as Unit;
    }

    public override IEnumerator ReceiveDamage(int amount)
    {
        bool defeated = combatPieceStats.ReceiveDamage(amount, stackStats, out int stackLost);
        //string log = unit.GetName() + " took " + amount + " damage. " + stackLost + " units died.";
        //CombatManager.Instance.AddEntryToLog(log);
        if (defeated) yield return StartCoroutine(DamagedDead());
        else yield return StartCoroutine(DamagedHurt());
    }

    public override void Die()
    {
        stackStats.Subtract(stackStats.Get());
        base.Die();
    }
}
