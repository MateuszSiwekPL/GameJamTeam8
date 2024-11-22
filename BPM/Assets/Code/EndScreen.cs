using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resultText;
    public void Setup(bool win)
    {
        _resultText.text = win ? "You Win!" : "You Lose!";
    }
    
}
