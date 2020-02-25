using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePickupPiece2 : AbstractPickupPiece2
{
    [Header("Resource pickup?")]
    public ResourceType resourceType;
    public int resourceAmount;

    public void Initialize(ResourceType resourceType, int resourceAmount)
    {
        pickupType = PickupType.RESOURCE;
        this.resourceType = resourceType;
        this.resourceAmount = resourceAmount;

        name = "Resource pickup: " + resourceAmount + " units of " + resourceType;
    }

    protected override void AP2_UpdateAnimatorParameters()
    {
        //throw new System.NotImplementedException();   //not yet...
    }

    public override string AFP2_GetPieceTitle()
    {
        return resourceType + " (" + resourceAmount + ")";
    }
}
