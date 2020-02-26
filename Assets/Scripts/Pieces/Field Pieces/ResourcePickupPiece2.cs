using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePickupPiece2 : AbstractPickupPiece2
{
    [Header("Resource pickup?")]
    public DB_Resource dbResource;
    public int resourceAmount;

    public void Initialize(DB_Resource dbResource, int resourceAmount)
    {
        name = "Resource pickup: " + resourceAmount + " units of " + dbResource.resourceName;
        pickupType = PickupType.RESOURCE;

        this.dbResource = dbResource;
        this.resourceAmount = resourceAmount;

        SetMainSprite(dbResource.spritePickup, SpriteOrderConstants.PIECE);
    }

    protected override void AP2_UpdateAnimatorParameters()
    {
        //throw new System.NotImplementedException();   //not yet...
    }

    public override string AFP2_GetPieceTitle()
    {
        return dbResource.resourceName + " (" + resourceAmount + ")";
    }
}
