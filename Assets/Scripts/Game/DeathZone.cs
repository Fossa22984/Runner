using System.Threading.Tasks;
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
        }
    }
}