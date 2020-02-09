using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI : AbstractSingleton<TownUI>, IUIScheme, IShowableHideable
{
    [Header("Town components")]
    public Image background;
    public RectTransform buildingsHolder;
    public List<TownUI_Building> tuiBuildings = new List<TownUI_Building>();

    [Header("Panels")]
    public TownUI_Panel_CoreButtons coreButtons;
    public TownUI_Panel_Resources resources;
    public TownUI_Panel_Timers timers;
    public TownUI_Panel_Minimap minimap;
    public TownUI_Panel_Town townPanel;
    public TownUI_Panel_Parties parties;
    public TownUI_Panel_Crest crest;

    //[Header("Windows")]
    //public FieldUI_Panel_EscapeMenu escapeMenu;
    //public FieldUI_Panel_Inventory inventory;

    [Header("Current Window")]
    public AUIPanel currentWindow;

    public void Hide()
    {
        gameObject.SetActive(false);

        background.gameObject.SetActive(false);
        buildingsHolder.gameObject.SetActive(false);

        coreButtons.Hide();
        resources.Hide();
        timers.Hide();
        minimap.Hide();
        parties.Hide();
        townPanel.Hide();
        crest.Hide();

        //EscapeMenuHide();
        //InventoryHide();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        background.gameObject.SetActive(true);
        buildingsHolder.gameObject.SetActive(true);

        coreButtons.Show();
        resources.Show();
        timers.Show();
        minimap.Show();
        parties.Show();
        townPanel.Show();
        crest.Show();
    }

    public void CloseCurrentWindow()
    {
        throw new System.NotImplementedException();
    }

    public void UpdatePanels()
    {
        Debug.Log("TownUI PANELS BEING UPDATED ;-)");
    }

    public void DestroyTownBuildings()
    {
        foreach (TownUI_Building item in tuiBuildings) Destroy(item.gameObject);
        tuiBuildings.Clear();
    }

    public void CreateTownBuildings(List<TownBuilding> townBuildings)
    {
        DestroyTownBuildings();

        TownUI_Building prefab = AllPrefabs.Instance.tuiBuilding;
        foreach (TownBuilding bldg in townBuildings)
        {
            TownUI_Building newTUI = Instantiate(prefab, buildingsHolder.transform);
            newTUI.townBuilding = bldg;

            newTUI.image.sprite = bldg.dbTownBuilding.image;

            tuiBuildings.Add(newTUI);
        }
    }
}
