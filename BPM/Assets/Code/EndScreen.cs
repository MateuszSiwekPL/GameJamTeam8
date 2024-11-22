using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    public void Setup(bool win, float score)
    {
        _resultText.text = win ? "You Win!" : "You Lose!";
        _scoreText.text = "Score: " + score.ToString("F0");
        _exitButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
        
        _restartButton.onClick.AddListener(() => { 
            var currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);
            });
    }
    
}
