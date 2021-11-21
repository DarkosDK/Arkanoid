using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ScoreBehavior : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetScoreText(int value)
    {
        text.SetText(value.ToString().PadLeft(4, '0'));
    }
}
