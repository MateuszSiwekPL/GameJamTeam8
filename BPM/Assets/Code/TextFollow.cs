using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFollow : MonoBehaviour
{
    public Transform player;
    public RectTransform textRectTransform;
    public Canvas canvas;
    public Camera cam;
    public Vector2 offset;

    void Update()
    {
        Vector3 screenPosition = cam.WorldToScreenPoint(player.position);
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.worldCamera,
            out Vector2 canvasPosition
        );
        
        canvasPosition += offset;
        
        textRectTransform.anchoredPosition = canvasPosition;
    }
}
