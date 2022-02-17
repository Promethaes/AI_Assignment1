//Anthony Smiderle
//100695532
//2022/02/07
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCloseToDoor : MonoBehaviour
{
    [SerializeField] AudioSource source = null;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        source.Play();
    }
}
