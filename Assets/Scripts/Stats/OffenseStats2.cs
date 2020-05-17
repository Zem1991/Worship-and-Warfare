using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffenseStats2 : MonoBehaviour
{
    public AttackStats2 attack_melee;
    public AttackStats2 attack_ranged;

    public void Initialize(AttackStats2 melee, AttackStats2 ranged)
    {
        if (attack_melee) Destroy(attack_melee.gameObject);
        attack_melee = Instantiate(melee);
        attack_melee.transform.parent = transform;

        if (attack_ranged) Destroy(attack_ranged.gameObject);
        attack_ranged = Instantiate(ranged);
        attack_ranged.transform.parent = transform;
    }
}
