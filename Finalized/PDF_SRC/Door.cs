//Anthony Smiderle
//100695532
//2022/02/07
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject hot = null;
    [SerializeField] GameObject noisy = null;
    [SerializeField] GameObject safe = null;

    public void SetHotNoisySafe(bool h, bool n, bool s)
    {
        hot.SetActive(h);
        noisy.SetActive(n);
        safe.SetActive(s);
    }
}
