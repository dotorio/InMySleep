using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEffect : MonoBehaviour
{
    private AudioSource effectSound;
    void Start()
    {
        effectSound = GetComponent<AudioSource>();
        effectSound.Play();
    }

}
