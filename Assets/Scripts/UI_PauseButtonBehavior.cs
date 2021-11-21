using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PauseButtonBehavior : MonoBehaviour
{
    [SerializeField] private Sprite _pauseSprite;

    [SerializeField] private Sprite _playSprite;

    private Image _buttonImage;


    void Start()
    {
        _buttonImage = GetComponent<Image>();

    }
    
    public void SetImage()
    {
        _buttonImage.sprite = GameManager.Instance.GetPause() ? _pauseSprite : _playSprite;
    }
}
