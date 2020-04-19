using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_Resources : AbstractUIPanel
{
    public Text txtGold;
    public Text txtOre;
    public Text txtAle;
    public Text txtCrystals;
    public Text txtSulphur;

    public void UpdatePanel(Player player)
    {
        txtGold.text = "" + player.currentResources.gold;
        txtOre.text = "" + player.currentResources.ore;
        txtAle.text = "" + player.currentResources.ale;
        txtCrystals.text = "" + player.currentResources.crystals;
        txtSulphur.text = "" + player.currentResources.sulphur;
    }
}
