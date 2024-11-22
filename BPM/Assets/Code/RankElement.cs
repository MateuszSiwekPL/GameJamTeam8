using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RankElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankText;
    [SerializeField] private TextMeshProUGUI _playerNameText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    public void SetRank(int rank, string playerName, int score)
    {
        _rankText.text = rank.ToString();
        _playerNameText.text = playerName;
        _scoreText.text = score.ToString();
    }
}
