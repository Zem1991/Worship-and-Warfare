using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatantHeroPiece2 : AbstractCombatantPiece2
{
    [Header("Hero data")]
    public Hero hero;

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

        AttributeStats prefabAS = AllPrefabs.Instance.attributeStats;
        //ExperienceStats prefabES = AllPrefabs.Instance.experienceStats;

        this.hero = hero;
        name = "P" + owner.id + " - " + hero.dbData.heroName + ", " + hero.dbData.classs.className;

        attributeStats = Instantiate(prefabAS, transform);
        attributeStats.Initialize(hero.attributeStats);

        IMP_ResetMovementPoints();
        SetAnimatorOverrideController(hero.dbData.classs.animatorCombat);
    }
}
