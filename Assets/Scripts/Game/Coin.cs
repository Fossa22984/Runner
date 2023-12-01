using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public event Action<GameObject> CoinRemoveEvent;
    public event Action SetCoinEvent;

    public bool IsSubscribedCoinRemoveEvent;
    public bool IsSubscribedSetCoinEvent;

    private void OnTriggerEnter(Collider other)
    {
        PoolManager.PutObject(gameObject);
        CoinRemoveEvent?.Invoke(gameObject);
        SetCoinEvent?.Invoke();
    }

    public bool CheckSubscribedCoinRemoveEvent() => CoinRemoveEvent != null;
    public bool CheckSubscribedSetCoinEvent() => SetCoinEvent != null;

    ~Coin()
    {
        if (CoinRemoveEvent != null)
        {
            foreach (Delegate d in CoinRemoveEvent.GetInvocationList())
            {
                CoinRemoveEvent -= (Action<GameObject>)d;
                IsSubscribedCoinRemoveEvent = CheckSubscribedCoinRemoveEvent();
            }
        }

        if (SetCoinEvent != null)
        {
            foreach (Delegate d in SetCoinEvent.GetInvocationList())
            {
                SetCoinEvent -= (Action)d;
                IsSubscribedSetCoinEvent = CheckSubscribedSetCoinEvent();
            }
        }
    }
}