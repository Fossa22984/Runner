using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public delegate Task ResetLevelDelegate();
    public ResetLevelDelegate ResetLevelEvent;

    private int _seconds = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetLevelEvent?.Invoke();
            //TimerCallback callback = state => ResetLevelEvent?.Invoke();
            //System.Threading.Timer timer = new System.Threading.Timer(callback, null, _seconds*1000, Timeout.Infinite);
        }
    }
}