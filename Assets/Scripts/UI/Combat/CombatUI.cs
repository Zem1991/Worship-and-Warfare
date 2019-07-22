using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI : Singleton<CombatUI>, IUIScheme
{
    public void UpdatePanels()
    {
        Debug.Log("CombatUI PANELS BEING UPDATED ;-)");
    }
}
