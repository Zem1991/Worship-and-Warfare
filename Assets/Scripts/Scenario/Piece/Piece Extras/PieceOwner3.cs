using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceOwner3 : MonoBehaviour
{
    [SerializeField] private Player owner;

    public Player GetOwner()
    {
        return owner;
    }

    public void SetOwner(Player player)
    {
        TownPiece3 thisAsTown = GetComponent<TownPiece3>();
        PartyPiece3 thisAsParty = GetComponent<PartyPiece3>();
        if (owner)
        {
            if (thisAsTown) owner.townPieces.Remove(thisAsTown);
            if (thisAsParty) owner.partyPieces.Remove(thisAsParty);
        }

        owner = player;
        if (owner)
        {
            if (thisAsTown) owner.townPieces.Add(thisAsTown);
            if (thisAsParty) owner.partyPieces.Add(thisAsParty);
        }
    }
}
