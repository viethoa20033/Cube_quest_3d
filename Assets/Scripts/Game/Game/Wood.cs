using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    public Rigidbody rb;
    private AudioSource source;

    public bool isMove;
    public bool isTarget;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (rb.velocity.magnitude > .1f)
        {
            isMove = true;
            if (!source.isPlaying)
            {
                source.Play();
            }
        }
        else
        {
            isMove = false;
            source.Stop();
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Target") && !isMove)
        {
            isTarget = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            isTarget = false;
        }
    }
}
