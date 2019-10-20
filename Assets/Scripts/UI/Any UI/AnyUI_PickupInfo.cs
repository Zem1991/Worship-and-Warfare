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
                    portrait.sprite = pickup.artifact.dbData.image;
                    txtPickupName.text = pickup.artifact.dbData.artifactName;
                    txtPickupDescription.text = pickup.artifact.dbData.artifactDescription;
                    break;
                case PickupType.UNIT:
                    portrait.sprite = pickup.unit.dbData.profilePicture;
                    txtPickupName.text = pickup.unit.dbData.namePlural;
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
