using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStats2 : MonoBehaviour
{
    public int hitPoints_current;
    public int hitPoints_maximum;

    public void Awake()
    {
        //TODO: consider making an check start values on each Stats class
        if (hitPoints_current <= 0) hitPoints_current = 1;
        if (hitPoints_maximum <= 0) hitPoints_maximum = 1;
    }

    public virtual void CopyFrom(HealthStats2 healthStats)
    {
        hitPoints_current = healthStats.hitPoints_current;
        hitPoints_maximum = healthStats.hitPoints_maximum;
    }

    public virtual bool ReceiveHealing(int amount)
    {
        hitPoints_current += amount;
        hitPoints_current = Mathf.Clamp(hitPoints_current, 0, hitPoints_maximum);
        return hitPoints_current >= hitPoints_maximum;
    }

    public virtual bool ReceiveDamage(int amount)
    {
        hitPoints_current -= amount;
        hitPoints_current = Mathf.Clamp(hitPoints_current, 0, hitPoints_maximum);
        return hitPoints_current <= 0;
    }
}
