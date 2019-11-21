using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceOwner : MonoBehaviour
{
    [SerializeField] protected Player owner;

    public Player GetOwner()
    {
        return owner;
    }

    public void SetOwner(Player player)
    {
        owner = player;
    }
}
