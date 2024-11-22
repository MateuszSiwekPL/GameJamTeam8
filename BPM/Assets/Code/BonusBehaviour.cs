using UnityEngine;

public class BonusBehaviour : MonoBehaviour
{
        [SerializeField] private KeyCode _correctKey;
        [SerializeField] private float speed;
        
        private void Update()
        {
            transform.Translate(speed * Time.deltaTime * Vector3.down);
        }
        public void TryGetBonus(KeyCode keyCode)
        {
            if (keyCode == _correctKey)
            {
                GameManager.Instance.AddTime(10f);
                Destroy(gameObject);
            }
        }
}