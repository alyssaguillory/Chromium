using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraUtils
{

    /// <summary>
    /// Indicates that the sprite is out of screen bounds horizontally
    /// </summary>
    public static bool outOfView(SpriteRenderer renderer, Transform playerCam, float threshold = 0.5f)
    {
        var position = playerCam.position;
        double centerDistance = Mathf.Abs(renderer.bounds.center.x - position.x);
        double totalHalfWidth = Mathf.Abs(getWidth(renderer) + getCamWidth(playerCam));

        return centerDistance - totalHalfWidth > threshold;
        return renderer.bounds.center.x - getWidth(renderer) > position.x + getCamWidth(playerCam)
               || renderer.bounds.center.x + getWidth(renderer) < position.x - getCamWidth(playerCam);
    }

    /// <summary>
    /// Indicates that right edge of the sprite is within screen bounds
    /// </summary>
    public static bool rightEdgeIn(SpriteRenderer renderer, Transform playerCam, float threshold = 0.5f)
    {
        return renderer.bounds.center.x + getWidth(renderer) - threshold < playerCam.position.x + getCamWidth(playerCam) ;
    }

    /// <summary>
    /// Indicates that left edge of the sprite is within screen bounds
    /// </summary>
    public static bool leftEdgeIn(SpriteRenderer renderer, Transform playerCam, float threshold = 0.5f)
    {
        return renderer.bounds.center.x - getWidth(renderer) + threshold >= playerCam.position.x - getCamWidth(playerCam);
    }

    /// <summary>
    /// Screen width in world units
    /// </summary>
    public static float getCamWidth(Transform playerCam)
    {
        return playerCam.GetComponent<Camera>().orthographicSize * Screen.width / Screen.height;
    }

    /// <summary>
    /// Screen height in world units
    /// </summary>
    public static float getCamHeight(Transform playerCam)
    {
        return playerCam.GetComponent<Camera>().orthographicSize;
    }

    /// <summary>
    /// Spite width in world units
    /// </summary>
    public static float getWidth(SpriteRenderer sprite)
    {
        return sprite.bounds.size.x / 2;
    }

    /// <summary>
    /// Spite height in world units
    /// </summary>
    public static float getHeight(SpriteRenderer sprite)
    {
        return sprite.bounds.size.y / 2;
    }
}
