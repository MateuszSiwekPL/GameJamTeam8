using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitObjectBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.left, Space.World);
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
    
    public void RemoveBit()
    {
        var clr = spriteRenderer.color;
        clr.a = 0;
        spriteRenderer.color = clr;
    }
}
