using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformMove : MonoBehaviour
{
    public float border;

    private Camera _mainCamera;

    private float _posY;

    private float _leftBorder;

    private float _rightBorder;
    private SpriteRenderer _sprite;
    private float _halfSpriteSize;
    private bool _isActive = true;
    

    void Start()
    {
        _mainCamera = Camera.main;
        _posY = transform.position.y;

        _sprite = GetComponent<SpriteRenderer>();

        UpdateBorders();

    }
    
    
    void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (_isActive && !GameManager.Instance.GetPause())
        {
            float posX = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 0)).x;

            posX = Mathf.Clamp(posX, _leftBorder, _rightBorder);
            transform.position = new Vector3(posX, _posY);  
        }
        
    }

    public void UpdateBorders()
    {
        _halfSpriteSize = _sprite.size.x / 2;
        _leftBorder = -2.8f + border + _halfSpriteSize;
        _rightBorder = 2.8f - border - _halfSpriteSize;
    }
    
}
