using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_Panel_LevelUp : AbstractUIPanel
{
    [Header("Announcement")]
    public Image imgPortrait;
    public Text txtAnnouncement;
    public FieldUI_Panel_LevelUp_AttributeOption fuiPrimaryAtt;
    public FieldUI_Panel_LevelUp_AttributeOption fuiRandomAtt;

    [Header("Attribute Increase")]
    public List<FieldUI_Panel_LevelUp_AttributeOption> attributeIncreaseOptions;

    [Header("Buttons")]
    public Button btnConfirm;

    [Header("Current selections")]
    public FieldUI_Panel_LevelUp_AttributeOption fuiSelectedAtt;

    private Hero hero;

    public void UpdatePanel(Hero hero)
    {
        DBHandler_Attribute dbAttributes = DBHandler_Attribute.Instance as DBHandler_Attribute;
        this.hero = hero;

        int nextLevel = hero.experienceStats.level + 1;

        imgPortrait.sprite = hero.dbData.profilePicture;
        txtAnnouncement.text = hero.dbData.heroName + " is now an level " + nextLevel + " " + hero.dbData.heroClass.className + "!";

        AttributeType primaryAtt = hero.dbData.heroClass.GetPrimaryAttribute();
        DB_Attribute primaryDB = dbAttributes.SelectFromType(primaryAtt);
        fuiPrimaryAtt.SetAttribute(primaryDB);

        AttributeType randomAtt = hero.LevelUp_SelectRandomAttribute();
        DB_Attribute randomDB = dbAttributes.SelectFromType(randomAtt);
        fuiRandomAtt.SetAttribute(randomDB);

        List<AttributeType> attChoices = hero.LevelUp_ListAttributeOptions(randomAtt);
        List<DB_Attribute> optionsDB = new List<DB_Attribute>();
        foreach (var item in attChoices) optionsDB.Add(dbAttributes.SelectFromType(item));
        if (attributeIncreaseOptions.Count != optionsDB.Count) Debug.LogError("Incompatible number of options!");
        for (int i = 0; i < attributeIncreaseOptions.Count; i++) attributeIncreaseOptions[i].SetAttribute(optionsDB[i]);
    }

    public void SelectOption(FieldUI_Panel_LevelUp_AttributeOption option)
    {
        if (option == fuiPrimaryAtt) return;
        if (option == fuiRandomAtt) return;

        fuiSelectedAtt = option;
        btnConfirm.interactable = true;
    }

    public void Confirm()
    {
        AttributeType randomAtt = fuiRandomAtt.attributeType;
        AttributeType choiceAtt = fuiSelectedAtt.attributeType;
        hero.LevelUp_ApplyLevelUp(randomAtt, choiceAtt);

        btnConfirm.interactable = false;
        fuiSelectedAtt = null;

        FieldUI.Instance.LevelUpHide();
    }
}
