using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandablePiece
{
    void ICP_StartTurn();
    void ICP_EndTurn();
    void ICP_InteractWithTile(AbstractTile aTile, bool canPathfind);
    void ICP_InteractWithPiece(AbstractPiece2 aPiece, bool canPathfind);
    bool ICP_IsIdle();
}
