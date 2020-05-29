using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceOwner3))]
[RequireComponent(typeof(PieceController3))]
public class TownPiece3 : AbstractFieldPiece3, IFlaggablePiece, IPieceForCombat
{
    [Header("Object components")]
    public SpriteRenderer flagSpriteRenderer;

    [Header("Object components")]
    public PieceOwner3 pieceOwner;
    public PieceController3 pieceController;

    [Header("Town references")]
    public Town town;
    public PartyPiece3 visitorPiece;

    public void Initialize(Player owner, Town town)
    {
        pieceOwner.SetOwner(owner);
        pieceController.SetController(owner);

        this.town = town;
        name = "Town Piece: " + town.townName;

        SetMainSprite(town.dbFaction.townFieldSprite, SpriteOrderConstants.PIECE);
        IFP_SetFlagSprite(owner.dbColor.imgFlag);
    }

    protected override void AP3_UpdateAnimatorParameters()
    {
        //TODO: throw new System.NotImplementedException();
    }

    public override string AFP3_GetPieceTitle()
    {
        return town.townName;
    }

    public void IFP_SetFlagSprite(Sprite sprite)
    {
        flagSpriteRenderer.sprite = sprite;
        flagSpriteRenderer.sortingOrder = SpriteOrderConstants.FLAG;
    }

    public Player IPFC_GetPlayerForCombat()
    {
        return pieceOwner.GetOwner();
    }

    public Party IPFC_GetPartyForCombat()
    {
        return town.GetGarrison();
    }

    public Town IPFC_GetPTownForCombat()
    {
        return town;
    }
}
