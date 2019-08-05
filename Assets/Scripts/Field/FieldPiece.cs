using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPiece : AbstractPiece
{
    [Header("Party Contents")]
    public Hero hero;
    public Unit[] units;

    public override void InteractWithPiece(AbstractPiece target)
    {
        FieldManager.Instance.pieceHandler.PiecesAreInteracting(this, target as FieldPiece);
    }
}
