using Assets.Scripts.DataBase;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class GameSpeedController : MonoBehaviour
{
    public static float Speed { get; private set; } = Helper.StartingSpeed;

    [SerializeField] private Menu _menu;
    [SerializeField] private PlayerRepository _player;

    private float _speedChange = Helper.SpeedChangeStep;
    private float _elapsedTime = 0;

    void Update()
    {
        if (GameManager.IsPause) return;

        _elapsedTime += Time.deltaTime;
        var score = (int)(_elapsedTime * Speed);
        if (_elapsedTime >= _speedChange)
        {
            Speed++;
            _speedChange += Helper.SpeedChangeStep;
        }

        _menu.SetScore(score, _player.Player.Score.BestScore);
        _player.SetBestScore(score);
    }

    public void Reset()
    {
        Speed = Helper.StartingSpeed;
        _speedChange = Helper.SpeedChangeStep;
        _elapsedTime = 0;
    }
}