using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatantHeroPiece2 : AbstractCombatantPiece2
{
    [Header("UI references")]
    public RectTransform uiBarRect;
    public Image uiHealthBar;
    public Image uiManaBar;

    [Header("Prefab references")]
    public AttributeStats attributeStats;
    //public ExperienceStats experienceStats;
    //public Inventory inventory;

    protected override void Update()
    {
        base.Update();

        bool showUI = !stateDead && ICP_IsIdle();
        uiBarRect.gameObject.SetActive(showUI);
        uiHealthBar.fillAmount = ((float)combatPieceStats.hitPoints_current) / combatPieceStats.hitPoints_maximum;
        uiManaBar.fillAmount = 0.8F;
    }

    public void Initialize(Hero hero, Player owner, int spawnId, bool defenderSide = false)
    {
        Initialize(owner, hero.combatPieceStats, spawnId, defenderSide);

        partyElement = hero;
        name = "P" + owner.id + " - " + hero.dbData.heroName + ", " + hero.dbData.heroClass.className;

        attributeStats.Copy(hero.attributeStats);

        IMP_ResetMovementPoints();
        SetAnimatorOverrideController(hero.dbData.heroClass.animatorCombat);
    }

    public Hero GetHero()
    {
        return partyElement as Hero;
    }
}
