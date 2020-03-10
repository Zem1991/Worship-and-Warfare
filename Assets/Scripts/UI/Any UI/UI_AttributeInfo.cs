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

    public void RefreshInfo(AttributeStats attributeStats)
    {
        if (attributeStats != null)
        {
            txtOffense.text = attributeStats.atrOffense.ToString();
            txtDefense.text = attributeStats.atrDefense.ToString();
            txtSupport.text = attributeStats.atrSupport.ToString();
            txtCommand.text = attributeStats.atrCommand.ToString();
            txtMagic.text = attributeStats.atrMagic.ToString();
            txtTech.text = attributeStats.atrTech.ToString();
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
