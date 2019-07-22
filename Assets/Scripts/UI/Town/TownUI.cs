using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI : Singleton<TownUI>, IUIScheme
{
    public void UpdatePanels()
    {
        Debug.Log("TownUI PANELS BEING UPDATED ;-)");
    }
}
