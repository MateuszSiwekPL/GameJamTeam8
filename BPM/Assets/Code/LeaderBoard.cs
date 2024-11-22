using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private GameObject rankPrefab;
    [SerializeField] private Transform rankParent;
    void Start()
    {
        var rankings = LoadRankings();
        foreach (var rank in rankings.Take(10))
        {
            Instantiate(rankPrefab, rankParent)
                .GetComponent<RankElement>()
                .SetRank(rankings.IndexOf(rank) + 1, rank.playerName, rank.score);
        }
    }
    
    private List<(string playerName, int score)> LoadRankings()
    {
        int rankCount = PlayerPrefs.GetInt("RankCount", 0);
        List<(string, int)> rankings = new List<(string, int)>();

        for (int i = 0; i < rankCount; i++)
        {
            string[] entry = PlayerPrefs.GetString($"Rank{i}").Split('|');
            rankings.Add((entry[0], int.Parse(entry[1])));
        }
        
        rankings = rankings.OrderByDescending(r => r.Item2).ToList();
        return rankings;
    }
}
