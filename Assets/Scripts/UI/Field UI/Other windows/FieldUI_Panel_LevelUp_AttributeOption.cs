using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FieldUI_Panel_LevelUp_AttributeOption : MonoBehaviour, IPointerClickHandler
{
    [Header("UI element references")]
    public Text txtName;
    public Image image;
    public AttributeType attributeType;

    [Header("Parent panel reference")]
    public FieldUI_Panel_LevelUp parentPanel;

    public void SetAttribute(DB_Attribute dbAttribute)
    {
        if (txtName) txtName.text = dbAttribute.attributeName;
        image.sprite = dbAttribute.sprite;
        attributeType = dbAttribute.attributeType;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        parentPanel.SelectOption(this);
    }
}
