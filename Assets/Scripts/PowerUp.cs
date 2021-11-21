using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public Material effectMaterial;

    [SerializeField] private float speed = 1.0f;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = Vector2.down * speed;
        
        GameManager.Instance.gameOverEvent.AddListener(DeletePowerUp);
        GameManager.Instance.updatePowerUps.AddListener(DeletePowerUp);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Platform") || other.CompareTag("Finish"))
        {
            Destroy(gameObject);
        }
        
    }

    private void DeletePowerUp()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.Instance.gameOverEvent.RemoveListener(DeletePowerUp);
        GameManager.Instance.updatePowerUps.RemoveListener(DeletePowerUp);
    }
}
