using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitObjectBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.left);
    }
}
