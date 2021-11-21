using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float ballSpeed;
    private Rigidbody2D _ballRb;
    
    void Start()
    {
        _ballRb = GetComponent<Rigidbody2D>();
    }

    // Set up Ball speed
    public void SetSpeed(float speed)
    {
        ballSpeed = speed;
        Vector2 oldVelocity = _ballRb.velocity.normalized;
        _ballRb.velocity = oldVelocity * ballSpeed;
    }

    public float GetSpeed()
    {
        return ballSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            // Change bounce direction depending on position collides point on the platform
            Vector3 platformPos = other.transform.position;
            Vector3 ballPos = transform.position;
            float dist = Mathf.Abs(platformPos.x - ballPos.x);
            float platformHalfSize = other.collider.bounds.size.x;

            Vector2 dir;
            if (ballPos.x < platformPos.x)
            {
                dir = new Vector2(-dist / platformHalfSize, 1).normalized;
                
            }
            else
            {
                dir = new Vector2(dist / platformHalfSize, 1).normalized;

            }
            if (_ballRb != null)
            {
                _ballRb.velocity = dir * ballSpeed;
            }
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy ball if fell down below platform
        if (other.gameObject.CompareTag("Finish"))
        {
            Destroy(gameObject);
        }
    }

    
}
