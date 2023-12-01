using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _scoreText;

    public void SetPlayerData(string name, int score)
    {
        _nameText.text = name;
        _scoreText.text = score.ToString();
    }
}