using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController3 : MonoBehaviour
{
    [SerializeField] private Player controller;

    public Player Get()
    {
        return controller;
    }

    public void Set(Player player)
    {
        controller = player;
    }
}
