using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoinGenerator : MonoBehaviour
{
    public delegate void SetCoinDelegate();
    public SetCoinDelegate SetCoinEvent;

    [SerializeField] private GameObject _coinPrefab;
    private List<GameObject> _activeCoins = new List<GameObject>();

    [SerializeField] private int coinsCountInItem = 10;
    [SerializeField] private float coinsHeight = 0.5f;

    void Start()
    {
        Reset();
    }

    void Update()
    {
        if (GameManager.IsPause) return;

        foreach (var chunk in _activeCoins)
            chunk.transform.position -= new Vector3(0, 0, GameManager.Speed * Time.deltaTime);

        if (_activeCoins.Count != 0 && _activeCoins[0].transform.position.z < ConstVar.ChunkDeleteDistance)
        {
            PoolManager.PutObject(_activeCoins[0]);
            _activeCoins.RemoveAt(0);
        }
    }

    public void Reset()
    {
        while (_activeCoins.Count > 0)
        {
            PoolManager.PutObject(_activeCoins[0]);
            _activeCoins.RemoveAt(0);
        }
    }

    private void RemoveCoin(GameObject coin)
    {
        _activeCoins.Remove(coin);
    }

    private void SetCoin()
    {
        SetCoinEvent?.Invoke();
    }


    public void CreateCoins(Style coinStyle, Vector3 position)
    {
        Vector3 coinPosition = Vector3.zero;

        if (coinStyle == Style.Line)
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                var coin = PoolManager.GetObject(_coinPrefab);
                if (coin.TryGetComponent<Coin>(out var res))
                {
                    res.CoinRemoveEvent += RemoveCoin;
                    res.SetCoinEvent += SetCoin;
                }
                coinPosition.y = coinsHeight;
                coinPosition.z = i * ((float)1);
                coin.transform.position = coinPosition + position;
                coin.SetActive(true);
                _activeCoins.Add(coin);
            }
        }
        else if (coinStyle == Style.Jump)
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                var coin = PoolManager.GetObject(_coinPrefab);
                if (coin.TryGetComponent<Coin>(out var res))
                {
                    res.CoinRemoveEvent += RemoveCoin;
                    res.SetCoinEvent += SetCoin;
                }
                coinPosition.y = Mathf.Max(-1 / 2f * Mathf.Pow(i, 2) + 2.5f, coinsHeight);
                coinPosition.z = i * ((float)1);
                coin.transform.position = coinPosition + position;
                coin.SetActive(true);
                _activeCoins.Add(coin);
            }
        }
    }
}