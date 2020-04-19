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

        TownPiece2 thisAsTown = GetComponent<TownPiece2>();
        PartyPiece2 thisAsParty = GetComponent<PartyPiece2>();

        if (thisAsTown) player.townPieces.Add(thisAsTown);
        else if (thisAsParty) player.partyPieces.Add(thisAsParty);
    }
}
