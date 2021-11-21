using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ScoreTextBehavior : MonoBehaviour
{
    public ScoreObject score;

    public TextMeshProUGUI _text;
    
    public void UpdateRecords()
    {
        string recordsText = score.GetScoreString();
        
        _text.SetText(recordsText);
    }
}
