using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Dotween : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _text.transform.DOScale(5, 1).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
