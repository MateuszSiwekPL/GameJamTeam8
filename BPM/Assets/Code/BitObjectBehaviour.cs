using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitObjectBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.left);
    }
    
    public void RemoveBit()
    {
        var clr = spriteRenderer.color;
        clr.a = 0;
        spriteRenderer.color = clr;
    }
}
