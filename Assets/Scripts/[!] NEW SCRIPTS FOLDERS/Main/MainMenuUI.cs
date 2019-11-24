using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : AbstractSingleton<MainMenuUI>, IUIController
{
    [Header("Panels")]
    public MainMenuUI_MainMenu mainMenu;

    public void HideUI()
    {
        mainMenu.Hide();
    }

    public void ShowUI()
    {
        mainMenu.Show();
    }
}
