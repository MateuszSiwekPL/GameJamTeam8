using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BonusBehaviour : MonoBehaviour
{
        [SerializeField] private float speed;
    

        private void Update()
        {
            transform.Translate(speed * Time.deltaTime * Vector3.down);
        }
        public void TryGetBonus()
        {
            GameManager.Instance.AddTime(10f);
            Destroy(gameObject);
        }
}