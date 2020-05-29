using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPieceForCombat
{
    Player IPFC_GetPlayerForCombat();
    Party IPFC_GetPartyForCombat();
    Town IPFC_GetPTownForCombat();
}
