using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandablePiece
{
    bool ICP_IsIdle();
    void ICP_Stop();
    //void ICP_InteractWith(AbstractTile aTile, bool canPathfind);
}
