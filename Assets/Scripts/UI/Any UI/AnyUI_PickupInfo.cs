using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnyUI_PickupInfo : MonoBehaviour, IShowableHideable
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

    public void RefreshInfo(PickupPiece2 pickup)
    {
        if (pickup != null)
        {
            switch (pickup.pickupType)
            {
                case PickupType.RESOURCE:
                    Debug.LogWarning("Resource pickup is not supported");
                    break;
                case PickupType.ARTIFACT:
                    portrait.sprite = pickup.dbArtifact.image;
                    txtPickupName.text = pickup.dbArtifact.artifactName;
                    txtPickupDescription.text = pickup.dbArtifact.artifactDescription;
                    break;
                case PickupType.UNIT:
                    portrait.sprite = pickup.dbUnit.profilePicture;
                    txtPickupName.text = pickup.dbUnit.namePlural;  //TODO NAME IN SINGULAR/PLURAL
                    txtPickupDescription.text = pickup.unitAmount + " to be picked up.";
                    break;
            }
        }
        else
        {
            portrait.sprite = null;
            txtPickupName.text = "--";
            txtPickupDescription.text = "--";
        }
    }
}
