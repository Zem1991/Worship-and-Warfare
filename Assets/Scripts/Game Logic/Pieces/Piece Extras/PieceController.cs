using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    [SerializeField] protected Player controller;

    public Player GetController()
    {
        return controller;
    }

    public void SetController(Player player)
    {
        controller = player;
    }
}
