using UnityEngine;

[System.Serializable]
public class InitializePoolData
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public int Count { get; private set; }
}
