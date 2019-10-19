using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovablePiece
{
    void IMP_ResetMovementPoints();
    void IMP_Move();
    void IMP_Stop();
}
