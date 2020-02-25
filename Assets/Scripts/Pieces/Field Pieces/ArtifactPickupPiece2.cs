using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPickupPiece2 : AbstractPickupPiece2
{
    [Header("Artifact pickup")]
    public DB_Artifact dbArtifact;

    public void Initialize(DB_Artifact dbArtifact)
    {
        name = "Artifact pickup: " + dbArtifact.artifactName;
        pickupType = PickupType.ARTIFACT;

        this.dbArtifact = dbArtifact;

        SetMainSprite(dbArtifact.image, SpriteOrderConstants.PIECE);
    }

    protected override void AP2_UpdateAnimatorParameters()
    {
        //throw new System.NotImplementedException();   //not yet...
    }

    public override string AFP2_GetPieceTitle()
    {
        return dbArtifact.artifactName;
    }
}
