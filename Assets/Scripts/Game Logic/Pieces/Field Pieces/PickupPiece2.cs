using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPiece2 : AbstractFieldPiece2
{
    [Header("Pickup data")]
    public PickupType pickupType;

    [Header("Resource pickup?")]
    public ResourceType resourceType;
    public int resourceAmount;

    [Header("Artifact pickup?")]
    public DB_Artifact dbArtifact;

    [Header("Unit pickup?")]
    public DB_Unit dbUnit;
    public int unitAmount;

    public void Initialize(ResourceType resourceType, int resourceAmount)
    {
        pickupType = PickupType.RESOURCE;
        this.resourceType = resourceType;
        this.resourceAmount = resourceAmount;

        name = "Resource pickup: " + resourceAmount + " units of " + resourceType;
    }

    public void Initialize(DB_Artifact dbArtifact)
    {
        name = "Artifact pickup: " + dbArtifact.artifactName;
        pickupType = PickupType.ARTIFACT;

        this.dbArtifact = dbArtifact;

        SetMainSprite(dbArtifact.image);
    }

    public void Initialize(DB_Unit dbUnit, int unitAmount)
    {
        name = "Unit pickup: " + unitAmount + " " + dbUnit.namePlural;  //TODO NAME IN SINGULAR/PLURAL
        pickupType = PickupType.UNIT;

        this.dbUnit = dbUnit;
        this.unitAmount = unitAmount;
    }

    public override void AP2_AnimatorParameters()
    {
        //Nothing yet...
    }

    public override void AP2_PieceInteraction()
    {
        //Nothing yet...
    }
}
