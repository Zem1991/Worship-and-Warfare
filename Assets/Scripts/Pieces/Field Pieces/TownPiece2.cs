using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceOwner))]
[RequireComponent(typeof(PieceController))]
public class TownPiece2 : AbstractFieldPiece2, IStartTurnEndTurn
{
    [Header("Other references")]
    public PieceOwner pieceOwner;
    public PieceController pieceController;

    [Header("Town contents")]
    public Town town;
    public PartyPiece2 visitorPiece;

    protected override void ManualAwake()
    {
        base.ManualAwake();

        pieceOwner = GetComponent<PieceOwner>();
        pieceController = GetComponent<PieceController>();
    }

    public void Initialize(Player owner, Town town)
    {
        ManualAwake();

        this.town = town;

        pieceOwner.SetOwner(owner);
        pieceController.SetController(owner);

        name = "P" + owner.id + " - Town - " + town.townName;

        SetMainSprite(town.dbFaction.townFieldSprite, SpriteOrderConstants.PIECE);
        SetFlagSprite(owner.dbColor.imgFlag);
    }

    protected override void AP2_UpdateAnimatorParameters()
    {
        //Nothing yet...
    }

    public override string AFP2_GetPieceTitle()
    {
        return town.townName;
    }

    public void ISTET_StartTurn()
    {
        throw new NotImplementedException();
    }

    public void ISTET_EndTurn()
    {
        throw new NotImplementedException();
    }
}
