using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI_MainMenu : MonoBehaviour, IShowableHideable     //AUIPanel
{
    public Button btnTestScenario;
    public Button btnQuitGame;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
