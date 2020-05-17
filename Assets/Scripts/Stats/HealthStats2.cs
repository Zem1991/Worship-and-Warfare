using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStats2 : MonoBehaviour
{
    public int hitPoints_current;
    public int hitPoints_maximum;

    //public void Initialize(int current)
    //{
    //    hitPoints_current = current;
    //    hitPoints_maximum = current;
    //}

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
