using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private Rewarded _rewarded;
    [SerializeField] private Leaderboard _leaderboard;
    [SerializeField] private GameObject _gameOverView;
    [SerializeField] private GameObject _playButton;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _balanceText;

    private string _scoreFormat;
    private string _balanceFormat;

    private void Awake()
    {
        _scoreFormat = _scoreText.text;
        _balanceFormat = _balanceText.text;
    }

    public void SetData(int currentScore, int score, int coins)
    {
        _scoreText.text = string.Format(_scoreFormat, currentScore.ToString(), score.ToString(), coins.ToString());
        _balanceText.text = string.Format(_balanceFormat, coins.ToString());
    }

    public void SetCoins(int coins) =>
        _balanceText.text = string.Format(_balanceFormat, coins.ToString());

    public void SetScore(int currentScore, int score) =>
        _scoreText.text = string.Format(_scoreFormat, currentScore.ToString(), score.ToString());

    public void ShowViewGameOver(List<Player> players)
    {
        _rewarded.LoadAd();
        _leaderboard.ShowLeaderboard(players);
        _gameOverView.SetActive(true);
    }

    public void HideViewGameOver()
    {
        _leaderboard.HideLeaderboard();
        _gameOverView.SetActive(false);
        _playButton.SetActive(true);
    }
}