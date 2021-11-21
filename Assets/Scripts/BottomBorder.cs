using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BottomBorder : MonoBehaviour
{
    public UnityEvent ballLoseEvent;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            ballLoseEvent.Invoke();
        }
    }
}
