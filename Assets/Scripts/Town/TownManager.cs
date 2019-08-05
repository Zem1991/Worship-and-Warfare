﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : AbstractSingleton<TownManager>, IShowableHideable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
