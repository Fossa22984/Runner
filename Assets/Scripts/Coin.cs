using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed=100;
    // Start is called before the first frame update
    void Start()
    {
       // _rotationSpeed += Random.Range(0, _rotationSpeed / 4f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,_rotationSpeed*Time.deltaTime,0);
    }
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.gameObject.SetActive(false);
    }
}
