using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandablePiece
{
    bool ICP_IsIdle();
    void ICP_Stop();
    void ICP_InteractWith(AbstractTile tile);
    IEnumerator ICP_InteractWithTargetTile(AbstractTile targetTile, bool endTurnWhenDone);
    IEnumerator ICP_InteractWithTargetPiece(AbstractPiece3 targetPiece);
}
