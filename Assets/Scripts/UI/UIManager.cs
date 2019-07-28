using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("Current Scheme")]
    public IUIScheme scheme;

    [Header("Focused Panel")]
    public AUIPanel focusedPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (scheme != null)
        {
            scheme.UpdatePanels();
        }
    }

    public void ChangeScheme(GameScheme gs)
    {
        switch (gs)
        {
            case GameScheme.FIELD:
                scheme = FieldUI.Instance;
                break;
            case GameScheme.TOWN:
                scheme = TownUI.Instance;
                break;
            case GameScheme.COMBAT:
                scheme = CombatUI.Instance;
                break;
            default:
                Debug.LogWarning("No UI scheme found!");
                scheme = null;
                break;
        }
    }

    public void PointerEnter(AUIPanel panel)
    {
        focusedPanel = panel;
    }

    public void PointerExit(AUIPanel panel)
    {
        if (focusedPanel == panel) focusedPanel = null;
    }
}
