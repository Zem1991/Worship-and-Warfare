using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class AUIPanel : MonoBehaviour, IShowableHideable, IPointerEnterHandler, IPointerExitHandler
{
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.PointerEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.PointerExit(this);
    }
}
