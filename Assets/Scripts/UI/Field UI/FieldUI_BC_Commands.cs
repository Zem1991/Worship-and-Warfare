using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI_BC_Commands : AUIPanel
{
    public Text txtMovePoints;
    public Button btnMovement;
    public Button btnInventory;
    public Button btnSpellBook;
    public Text txtManaPoints;

    public void UpdatePanel(AbstractFieldPiece2 p)
    {
        PartyPiece2 pp = p as PartyPiece2;

        if (pp)
        {
            PieceMovement pm = pp.IMP_GetPieceMovement();
            txtMovePoints.text = pm.movementPointsCurrent + "/" + pm.movementPointsMax;
        }
    }
}
