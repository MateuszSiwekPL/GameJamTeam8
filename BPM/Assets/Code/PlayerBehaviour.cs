using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public BonusBehaviour CurrentBonus;
    public float Speed;
    public BoxCollider2D BoxCollider2D;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            other.GetComponent<EnemyBehaviour>().RemoveTime();
        }
        
        if(other.gameObject.CompareTag("Bonus"))
        {
            CurrentBonus = other.gameObject.GetComponent<BonusBehaviour>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Bonus"))
        {
            CurrentBonus = null;
        }
    }
}
