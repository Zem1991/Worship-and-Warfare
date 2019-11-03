using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AttributeInfo : MonoBehaviour, IShowableHideable
{
    public Text txtCommand;
    public Text txtOffense;
    public Text txtDefense;
    public Text txtPower;
    public Text txtFocus;

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
            txtCommand.text = attributeStats.atrCommand.ToString();
            txtOffense.text = attributeStats.atrOffense.ToString();
            txtDefense.text = attributeStats.atrDefense.ToString();
            txtPower.text = attributeStats.atrPower.ToString();
            txtFocus.text = attributeStats.atrFocus.ToString();
        }
        else
        {
            txtCommand.text = "--";
            txtOffense.text = "--";
            txtDefense.text = "--";
            txtPower.text = "--";
            txtFocus.text = "--";
        }
    }
}
