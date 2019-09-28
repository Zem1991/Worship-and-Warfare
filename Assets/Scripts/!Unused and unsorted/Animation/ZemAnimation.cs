using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ZemAnimation : MonoBehaviour
{
    [Header("Objects")]
    public SpriteRenderer renderer;

    [Header("Current animation")]
    public ZemAnimationClip currentClip;
    public float currentClipDuration;
    public float currentClipDurationMax;
    public float currentClipFrame;
    public float currentClipFrameMax;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentClip != null) Animate();
    }

    private void Animate()
    {
        //TODO se der erro na linha abaixo, provavelmente é só mudar para '>='
        if (currentClipDuration > currentClipDurationMax) currentClipDuration = 0;

        float timePerFrame = currentClipDurationMax / currentClipFrameMax;
        int frameId = Mathf.FloorToInt(currentClipDuration / timePerFrame);
        Sprite frame = currentClip.sprites[frameId];
        renderer.sprite = frame;

        currentClipDuration += Time.deltaTime;
    }
}
