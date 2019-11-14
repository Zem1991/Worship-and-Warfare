using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandablePiece
{
    bool ICP_IsIdle();
    void ICP_Stop();
    void ICP_InteractWith(AbstractTile tile, bool canPathfind);
    void ICP_InteractWithTargetTile(AbstractTile targetTile, bool canPathfind);
    void ICP_InteractWithTargetPiece(AbstractPiece2 targetPiece, bool canPathfind);
}
