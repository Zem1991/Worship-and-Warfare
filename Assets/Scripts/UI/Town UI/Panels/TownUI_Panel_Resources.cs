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
        txtGold.text = "" + player.resourceStats.gold;
        txtOre.text = "" + player.resourceStats.ore;
        txtAle.text = "" + player.resourceStats.ale;
        txtCrystals.text = "" + player.resourceStats.crystals;
        txtSulphur.text = "" + player.resourceStats.sulphur;
    }
}
