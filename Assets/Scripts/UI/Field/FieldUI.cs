using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldUI : Singleton<FieldUI>, IUIScheme
{
    public void UpdatePanels()
    {
        Debug.Log("FieldUI PANELS BEING UPDATED ;-)");
    }
}
