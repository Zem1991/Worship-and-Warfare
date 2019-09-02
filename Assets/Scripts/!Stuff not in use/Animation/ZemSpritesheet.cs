using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZemSpritesheet : MonoBehaviour
{
    [Header("Identification")]
    public string id;

    [Header("Input")]
    public Texture2D originalImage;
    public int frameWidth;
    public int frameHeight;

    [Header("Output")]
    public int totalSprites;
    public List<Sprite> sprites = new List<Sprite>();

    public void CreateSprites(string id, Texture2D originalImage, int frameWidth, int frameHeight)
    {
        this.id = id;
        this.originalImage = originalImage;
        this.frameWidth = frameWidth;
        this.frameHeight = frameHeight;
        MakeSprites();
    }

    private void MakeSprites()
    {
        int imgWidth = originalImage.width;
        int imgHeight = originalImage.height;

        int framesGoingX = imgWidth / frameWidth;
        framesGoingX++;
        int framesGoingY = imgHeight / frameHeight;
        framesGoingY++;

        int posX;
        int posY;
        int pixelsX;
        int pixelsY;
        for (int row = 0; row < framesGoingY; row++)
        {
            for (int col = 0; col < framesGoingX; col++)
            {
                posX = col * frameWidth;
                posY = row * frameHeight;
                pixelsX = frameWidth;
                pixelsY = frameHeight;
                //pixelsX = imgWidth - (posX + frameWidth);
                //pixelsY = imgHeight - (posY + frameWidth);
                //endX = posX + Mathf.Min(frameWidth, pixelsX);
                //endY = posY + Mathf.Min(frameWidth, pixelsY);
                MakeSprite(posX, posY, pixelsX, pixelsY);
            }
        }

        totalSprites = sprites.Count;
    }

    private void MakeSprite(int x, int y, int width, int height)
    {
        Rect rect = new Rect(x, y, width, height);
        Vector2 pivot = Vector2.one * 0.5F;
        Sprite sprite = Sprite.Create(originalImage, rect, pivot);
        sprites.Add(sprite);
    }
}
