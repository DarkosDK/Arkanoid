using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_CurrentScoreTextBehavior : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Set Current Score text
    public void SetScoreText(int score)
    {
        _text.SetText("Current Result:\n" + score.ToString());
    }
}
