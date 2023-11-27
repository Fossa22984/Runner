using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    private string _scoreFormat;

    private void Awake()
    {
        _scoreFormat = _scoreText.text;
    }

    public void SetPlayerData(string name,int score)
    {
        _scoreFormat = _scoreText.text;
        _scoreText.text = string.Format(_scoreFormat, name, score.ToString());
    }
}