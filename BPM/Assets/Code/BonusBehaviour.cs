using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BonusBehaviour : MonoBehaviour
{
        [SerializeField] private float speed;
        
        [SerializeField] private GameObject _arrowLeft;
        [SerializeField] private GameObject _arrowRight;
        [SerializeField] private GameObject _arrowDown;
        [SerializeField] private GameObject _arrowUp;
        
        private KeyCode _correctKey;

        private void Start()
        { 
            KeyCode[] arrowKeys = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
            var randomIndex = Random.Range(0, arrowKeys.Length);
            _correctKey = arrowKeys[randomIndex];
            
            switch (_correctKey)
            {       
                case KeyCode.LeftArrow:
                    _arrowLeft.SetActive(true);
                    break;
                case KeyCode.RightArrow:
                    _arrowRight.SetActive(true);
                    break;
                case KeyCode.DownArrow:
                    _arrowDown.SetActive(true);
                    break;
                case KeyCode.UpArrow:
                    _arrowUp.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

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