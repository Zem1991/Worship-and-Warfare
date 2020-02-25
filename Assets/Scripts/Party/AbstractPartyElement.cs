using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPartyElement : MonoBehaviour
{
    public PartyElementType partyElementType;

    public abstract Sprite GetProfileImage();
}
