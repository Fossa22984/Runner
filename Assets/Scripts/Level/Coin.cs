using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 100;

    public delegate void CoinRemoveDelegate(GameObject coin);
    public CoinRemoveDelegate CoinRemoveEvent;

    void Start()
    {
        // _rotationSpeed += Random.Range(0, _rotationSpeed / 4f);
    }

    void Update()
    {
       // transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // PoolManager.PutObject(gameObject);

        PoolManager.PutObject(gameObject);
        CoinRemoveEvent?.Invoke(gameObject);
    }
}