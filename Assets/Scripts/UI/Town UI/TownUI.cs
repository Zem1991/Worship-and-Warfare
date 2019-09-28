using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI : AbstractSingleton<TownUI>, IUIScheme, IShowableHideable
{
    public override void Awake()
    {
        base.Awake();
        //EscapeMenuHide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void CloseCurrentWindow()
    {
        throw new System.NotImplementedException();
    }

    public void UpdatePanels()
    {
        Debug.Log("TownUI PANELS BEING UPDATED ;-)");
    }
}
