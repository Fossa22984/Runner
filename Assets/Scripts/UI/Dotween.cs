using DG.Tweening;
using TMPro;
using UnityEngine;

public class Dotween : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    void Start()
    {
        _text.transform.DOScale(1.5f, 1).SetLoops(-1, LoopType.Yoyo);
    }
}
