using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPiece3 : AbstractFieldPiece3
{
    [Header("Pickup type")]
    public PickupType pickupType;

    [Header("Resource pickup?")]
    public DB_Resource dbResource;
    public int resourceAmount;

    [Header("Artifact pickup")]
    public DB_Artifact dbArtifact;

    public void Initialize(DB_Resource dbResource, int resourceAmount)
    {
        name = "Resource pickup: " + dbResource.resourceName + " (x" + resourceAmount + ")";
        pickupType = PickupType.RESOURCE;

        this.dbResource = dbResource;
        this.resourceAmount = resourceAmount;

        SetMainSprite(dbResource.spritePickup, SpriteOrderConstants.PIECE);
    }

    public void Initialize(DB_Artifact dbArtifact)
    {
        name = "Artifact pickup: " + dbArtifact.artifactName;
        pickupType = PickupType.ARTIFACT;

        this.dbArtifact = dbArtifact;

        SetMainSprite(dbArtifact.image, SpriteOrderConstants.PIECE);
    }

    protected override void AP3_UpdateAnimatorParameters()
    {
        //throw new System.NotImplementedException();   //not yet...
    }

    public override string AFP3_GetPieceTitle()
    {
        string result = "Unknown";
        switch (pickupType)
        {
            case PickupType.RESOURCE:
                result = dbResource.resourceName + " (x" + resourceAmount + ")";
                break;
            case PickupType.ARTIFACT:
                result = dbArtifact.artifactName;
                break;
            default:
                break;
        }
        return result;
    }
}
