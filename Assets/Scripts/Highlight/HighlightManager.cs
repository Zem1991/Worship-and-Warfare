using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : AbstractSingleton<HighlightManager>
{
    [Header("Highlight Colors")]
    public Color highlightDefault = Color.white;
    public Color highlightAllowed = Color.green;
    public Color highlightDenied = Color.red;
}
