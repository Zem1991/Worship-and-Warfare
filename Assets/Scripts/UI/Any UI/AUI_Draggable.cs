using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AUI_Draggable : MonoBehaviour
{
    public Image image;

    public void Drag(Sprite img)
    {
        ChangeImage(img);
        transform.position = InputManager.Instance.mouseScreenPos;
    }

    public void EndDrag()
    {
        ChangeImage(null);
        transform.localPosition = Vector3.zero;
    }

    private void ChangeImage(Sprite img)
    {
        Color color = Color.white;
        if (!img) color.a = 0;
        image.color = color;
        image.sprite = img;
    }
}
