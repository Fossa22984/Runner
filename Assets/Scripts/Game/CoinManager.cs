using Assets.Scripts.DataBase;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private CoinGenerator _coinGenerator;
    [SerializeField] private Menu _menu;
    [SerializeField] private PlayerRepository _player;

    private void Awake()
    {
        _coinGenerator.SetCoinEvent += SetCoin;
    }

    private void SetCoin()
    {
        _player.SetBalance();
        _menu.SetCoins(_player.Player.Score.Balance);
    }
}