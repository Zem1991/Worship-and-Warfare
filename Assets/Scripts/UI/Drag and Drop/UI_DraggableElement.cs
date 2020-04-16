using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DraggableElement : MonoBehaviour
{
    public Image image;

    public void BeginDrag(Sprite img)
    {
        ChangeImage(img);
        Drag();
    }

    public void Drag()
    {
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
        if (!img)
        {
            //color.a = 0;

            //TODO remove those testing values later.
            //TODO also consider reseting the starting position.
            color = Color.magenta;
            color.a = 0.25F;
        }
        image.color = color;
        image.sprite = img;
    }
}
