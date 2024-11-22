using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timeToRemove;
    [SerializeField] private Behaviour behaviour;

    private enum Behaviour
    {
        TakeTime,
        RemoveBits
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.down);
    }

    public void RemoveTime()
    {
        if (behaviour == Behaviour.TakeTime)
        {
            GameManager.Instance.RemoveTime(timeToRemove);
        }
        else if (behaviour == Behaviour.RemoveBits)
        {
            GameManager.Instance.RemoveBits();
        }
    }
}
