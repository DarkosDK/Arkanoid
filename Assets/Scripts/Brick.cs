using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Brick : MonoBehaviour
{
    [HideInInspector]
    public BrickObject brickObject;
    
    [Header("Setup Brick")]
    public BrickType _brickType;

    [Range(1, 6)]
    [SerializeField] private int _health;

    // Start brick health
    private int _initHealth;

    [Header("Power Up")]
    public GameObject powerUpPrefab;

    // Cost of 1 health point
    [SerializeField] private int _pointCost = 10;
    
    private SpriteRenderer _spriteRenderer;

    private string _currentBrick = "";

    [SerializeField] private ParticleSystem _destroyEffect;


    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initHealth = _health;
        
        
    }

    private void Start()
    {
        GameManager.Instance.gameOverEvent.AddListener(RemoveBrick);
    }


    public int GetHealth()
    {
        return _health;
    }

    //Dynamic set brick texture depending of brick type (color)
    public BrickObject SetBrickTypeEditor()
    {
        if (_brickType.ToString() != _currentBrick)
        {
            string newTypeName = _brickType.ToString() + "Brick";
            BrickObject newType = Resources.Load<BrickObject>("BricksObjects/" + newTypeName);
            _currentBrick = _brickType.ToString();
            return newType;
        }
        else
        {
            return brickObject;
        }
  
    }

    // Get complete brick cost (points) depending of health 
    public int GetPoints()
    {
        return _pointCost * _initHealth;
    }

    // Reset all bricks for 0 level
    public void ResetHealth()
    {
        _initHealth = 1;
        SetHealth(1);
    }

    public void SetBrickTexture()
    {
        _spriteRenderer.sprite = brickObject.textures[_health - 1];
        SetEffectColor();
    }

    // Particle color
    private void SetEffectColor()
    {
        ParticleSystem.ColorOverLifetimeModule colorModule = _destroyEffect.colorOverLifetime;

        switch (brickObject.brickType)
        {
            case BrickType.Blue:
                colorModule.color = Color.blue;
                break;
            case BrickType.Green:
                colorModule.color = Color.green;
                break;
            case BrickType.Red:
                colorModule.color = Color.red;
                break;
            case BrickType.Yellow:
                colorModule.color = Color.yellow;
                break;
        }
    }

    //Change texture or delete (disable) brick in depends on its health
    private void SetHealth(int health)
    {
        _health = health;
        if (_health <= 0)
        {
            DeleteBrick();
        }
        else
        {
            SetBrickTexture();
        }

    }

    //Restore brick health (in order to increase it in next level)
    private void OnDisable()
    {
        _health = _initHealth;
    }

    //Increase brick health (when level grows), but not more than maximum possible
    public void IncreaseHealth()
    {
        _health = ++_initHealth;
        _health = Mathf.Clamp(_health, 0, 6);
    }

    //Disable brick and play effects
    public void DeleteBrick()
    {
        ParticleSystem p = Instantiate(_destroyEffect, transform.position, Quaternion.identity);
        p.Play();
        RemoveBrick();

        ActivatePowerUp();
        GameManager.Instance.DecreaseBricks();
        GameManager.Instance.AddPoint(GetPoints());
    }

    public void RemoveBrick()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Decrease health if collide with ball
        if (other.gameObject.CompareTag("Ball"))
        {
            AudioSource.PlayClipAtPoint(brickObject.hitAudio, new Vector3(0, 0, 0));
            SetHealth(_health - 1);
        }
    }

    private void ActivatePowerUp()
    {
        if (powerUpPrefab != null)
        {
            Vector2 newPos = new Vector2(transform.position.x, transform.position.y - 0.2f);
            Instantiate(powerUpPrefab, newPos, Quaternion.identity);
        }
    }
}
