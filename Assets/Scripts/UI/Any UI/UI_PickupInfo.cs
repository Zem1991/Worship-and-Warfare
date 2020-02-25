using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PickupInfo : MonoBehaviour, IShowableHideable
{
    public Image portrait;
    public Text txtPickupName;
    public Text txtPickupDescription;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void RefreshInfo(AbstractPickupPiece2 pickup, Text txtSelectionTitle)
    {
        if (pickup != null)
        {
            Sprite portraitSprite = null;
            string pickupName = null;
            string pickupDescription = null;

            switch (pickup.pickupType)
            {
                case PickupType.RESOURCE:
                    Debug.LogWarning("Resource pickup is not supported");
                    break;
                case PickupType.ARTIFACT:
                    ArtifactPickupPiece2 artifactPickup = pickup as ArtifactPickupPiece2;

                    portraitSprite = artifactPickup.dbArtifact.image;
                    pickupName = artifactPickup.dbArtifact.artifactName;
                    pickupDescription = artifactPickup.dbArtifact.artifactDescription;
                    break;
            }

            if (portrait) portrait.sprite = portraitSprite;
            if (txtPickupName) txtPickupName.text = pickupName;
            if (txtPickupDescription) txtPickupDescription.text = pickupDescription;

            txtSelectionTitle.text = pickupName;
        }
        else
        {
            if (portrait) portrait.sprite = null;
            if (txtPickupName) txtPickupName.text = "--";
            if (txtPickupDescription) txtPickupDescription.text = "--";

            txtSelectionTitle.text = "--";
        }
    }
}
