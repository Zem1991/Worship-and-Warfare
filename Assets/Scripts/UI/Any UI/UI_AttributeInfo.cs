using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AttributeInfo : MonoBehaviour, IShowableHideable
{
    public Text txtOffense;
    public Text txtDefense;
    public Text txtSupport;
    public Text txtCommand;
    public Text txtMagic;
    public Text txtTech;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void RefreshInfo(AttributeStats2 attributeStats)
    {
        if (attributeStats != null)
        {
            txtOffense.text = attributeStats.attributes.offense.ToString();
            txtDefense.text = attributeStats.attributes.defense.ToString();
            txtSupport.text = attributeStats.attributes.support.ToString();
            txtCommand.text = attributeStats.attributes.command.ToString();
            txtMagic.text = attributeStats.attributes.magic.ToString();
            txtTech.text = attributeStats.attributes.tech.ToString();
        }
        else
        {
            txtCommand.text = "--";
            txtOffense.text = "--";
            txtDefense.text = "--";
            txtMagic.text = "--";
            txtTech.text = "--";
        }
    }
}
