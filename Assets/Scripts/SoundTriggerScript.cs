using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTriggerScript : MonoBehaviour
{
    [SerializeField] private AudioSource machineSfxSource;
    [SerializeField] private AudioClip machineSfx;

    private void OnTriggerEnter(Collider other)
    {
        machineSfxSource.PlayOneShot(machineSfx);
    }
}
