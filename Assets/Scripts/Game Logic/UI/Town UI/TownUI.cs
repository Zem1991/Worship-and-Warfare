using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI : AbstractSingleton<TownUI>, IUIScheme, IShowableHideable
{
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void UpdatePanels()
    {
        Debug.Log("TownUI PANELS BEING UPDATED ;-)");
    }
}
