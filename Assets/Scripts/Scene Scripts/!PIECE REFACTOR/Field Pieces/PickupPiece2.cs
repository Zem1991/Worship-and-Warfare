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
    public Artifact artifact;

    [Header("Unit pickup?")]
    public Unit unit;
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
        Artifact prefab = AllPrefabs.Instance.artifact;
        Artifact artifact = Instantiate(prefab, transform);
        artifact.Initialize(dbArtifact);

        pickupType = PickupType.ARTIFACT;
        this.artifact = artifact;

        name = "Artifact pickup: " + artifact.dbData.artifactName;
        SetMainSprite(artifact.dbData.image);
    }

    public void Initialize(DB_Unit dbUnit, int unitAmount)
    {
        Unit prefab = AllPrefabs.Instance.unit;
        Unit unit = Instantiate(prefab, transform);
        unit.Initialize(dbUnit, unitAmount);

        pickupType = PickupType.UNIT;
        this.unit = unit;
        this.unitAmount = unitAmount;

        name = "Unit pickup: " + unitAmount + " " + dbUnit.namePlural;
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
