using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.down);
    }
}
