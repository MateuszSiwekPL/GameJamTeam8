using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timeToRemove;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.down);
    }

    public void RemoveTime()
    {
        GameManager.Instance.RemoveTime(timeToRemove);
    }
}
