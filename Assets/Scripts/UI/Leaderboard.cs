using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private LeaderboardItem _leaderboardItem;
    [SerializeField] private Transform _parentForLeaderboardItem;
    [SerializeField] private GameObject _leaderboard;

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
                Destroy(item.gameObject);
        }
    }

    private void CreatePlayerItem(Player player)
    {
        var playerItem = Instantiate(_leaderboardItem, _parentForLeaderboardItem);
        playerItem.SetPlayerData(player.Name, player.Score.BestScore);
    }
}