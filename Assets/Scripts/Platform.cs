using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Platform : MonoBehaviour
{
    public PowerUpEvent powerUpEvent;

    private Vector2 _initSpriteSize;
    private Vector2 _initColliderSize;
    
    private SpriteRenderer _sprite;
    private BoxCollider2D _collider;

    private PlatformMove _platformMove;
    
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();

        _initSpriteSize = _sprite.size;
        _initColliderSize = _collider.size;

        _platformMove = GetComponent<PlatformMove>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            powerUpEvent.Invoke(other.GetComponent<PowerUp>().type);
        }
    }

    public void StretchLength(float mult)
    {
        _initSpriteSize = _sprite.size;
        _initColliderSize = _collider.size;
        
        _sprite.size = new Vector2(_initSpriteSize.x * mult, _initSpriteSize.y);
        _collider.size = new Vector2(_initColliderSize.x * mult, _initColliderSize.y);
        
        //Update borders for Platform movement
        _platformMove.UpdateBorders();
    }
    
    
}

[System.Serializable]
public class PowerUpEvent : UnityEvent<PowerUpType> { };
    