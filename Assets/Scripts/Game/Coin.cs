using UnityEngine;

public class Coin : MonoBehaviour
{
    public delegate void CoinRemoveDelegate(GameObject coin);
    public CoinRemoveDelegate CoinRemoveEvent;

    public delegate void SetCoinDelegate();
    public SetCoinDelegate SetCoinEvent;

    private void OnTriggerEnter(Collider other)
    {
        PoolManager.PutObject(gameObject);
        CoinRemoveEvent?.Invoke(gameObject);

        SetCoinEvent?.Invoke();
    }
}