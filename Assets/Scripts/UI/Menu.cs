using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private LeaderboardItem _leaderboardItem;
    [SerializeField] private Transform _parentForLeaderboardItem;
    [SerializeField] private GameObject _leaderboard;

    [SerializeField] private TMP_Text _scoreText;
    private string _scoreFormat;
    private void Awake()
    {
        _scoreFormat = _scoreText.text;
    }

    public void SetData(int currentScore, int score, int coins)
    {
        _scoreText.text = string.Format(_scoreFormat, currentScore.ToString(), score.ToString(), coins.ToString());
    }

    public void ShowLeaderboard(List<Player> players)
    {
        foreach (var player in players)
            CreatePlayerItem(player);

        _leaderboard.SetActive(true);
    }
    public void HideLeaderboard()
    {
        _leaderboard.SetActive(false);


        foreach (Transform item in _parentForLeaderboardItem)
        {
            if (item.GetComponent<LeaderboardItem>() != null)
                Destroy(item);
        }
    }

    private void CreatePlayerItem(Player player)
    {
        var playerItem = Instantiate(_leaderboardItem, _parentForLeaderboardItem);
        playerItem.SetPlayerData(player.Name, player.Score.BestScore);
    }
}