using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : AbstractSingleton<CombatUI>, IUIScheme, IShowableHideable
{
    [Header("Panels")]
    public MainMenuUI_MainMenu mainMenu;

    public void Hide()
    {
        throw new System.NotImplementedException();
    }

    public void Show()
    {
        throw new System.NotImplementedException();
    }

    public void CloseCurrentWindow()
    {
        throw new System.NotImplementedException();
    }

    public void UpdatePanels()
    {
        throw new System.NotImplementedException();
    }
}
