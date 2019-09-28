using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZemAnimationClip
{
    public float duration = 1F;
    public List<Sprite> sprites = new List<Sprite>();

    public ZemAnimationClip(float duration, List<Sprite> sprites)
    {
        duration = Mathf.Clamp(duration, 0.1F, duration);

        this.duration = duration;
        this.sprites = sprites;
    }
}
