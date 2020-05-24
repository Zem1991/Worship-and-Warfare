using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AES_Action : MonoBehaviour        //TODO make an AES_SpellAction derived from this, to handle the mana costs
{
    [Header("Self references")]
    public AES_ActionFilters filters;
    public AES_ActionParameters parameters;

    public abstract IEnumerator Execute(CombatantPiece3 actionUser, List<AbstractTile> targetArea);
}
