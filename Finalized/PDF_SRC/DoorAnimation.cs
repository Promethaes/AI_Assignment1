//Anthony Smiderle
//100695532
//2022/02/07
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator animator = null;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        animator.SetTrigger("Open");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        animator.SetTrigger("Close");
    }
}
