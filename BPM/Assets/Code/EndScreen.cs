using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _winAudioSource;
    
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private Button _submitButton;
    
    private string _playerName;
    private int _score;
    public void Setup(bool win, float score)
    {
        if (!win)
        {
            _audioSource.Play();
        }
        else
        {
            _winAudioSource.Play();
        }
        _score = (int)score;
        _resultText.text = win ? "You Win!" : "You Lose!";
        _scoreText.text = "Score: " + score.ToString("F0");
        _exitButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
        
        _restartButton.onClick.AddListener(() => { 
            var currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);
            });

        SetInput();
    }
    
    private void SetInput()
    {
        _submitButton.interactable = false;
        
        _nameInput.onValueChanged.AddListener(delegate { ValidateName(); });
        
        _submitButton.onClick.AddListener(SubmitName);
    }
    
    void ValidateName()
    {
        _submitButton.interactable = !string.IsNullOrEmpty(_nameInput.text);
    }

    void SubmitName()
    {
        _playerName = _nameInput.text;
        
        SaveRanking(_playerName, _score);
    }

    void SaveRanking(string playerName, int score)
    {
        var rankCount = PlayerPrefs.GetInt("RankCount", 0);
        var newEntry = playerName + "|" + score;

        PlayerPrefs.SetString($"Rank{rankCount}", newEntry);
        PlayerPrefs.SetInt("RankCount", rankCount + 1);
        PlayerPrefs.Save();
        _submitButton.gameObject.SetActive(false);
    }
    
}
