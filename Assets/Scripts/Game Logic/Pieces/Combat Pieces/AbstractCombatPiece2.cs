using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCombatPiece2 : AbstractPiece2
{
    [Header("Hit point management")]
    public int hitPointsCurrent;
    public int hitPointsMax;
    public bool isDead;

    public virtual bool ACP_TakeDamage(int amount)
    {
        int amountFixed = Mathf.CeilToInt(amount);
        hitPointsCurrent -= amountFixed;

        if (hitPointsCurrent <= 0)
        {
            ACP_Die();
            return true;
        }
        return false;
    }

    public virtual void ACP_Die()
    {
        hitPointsCurrent = 0;
        isDead = true;
        mainSpriteRenderer.sortingOrder--;

        currentTile.occupantPiece = null;
        (currentTile as CombatTile).deadPieces.Add(this);
    }
}
