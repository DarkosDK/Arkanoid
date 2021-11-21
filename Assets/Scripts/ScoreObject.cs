using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "New Score Object")]


public class ScoreObject : ScriptableObject
{
    public int maximumNotes;

    private List<int> _score = new List<int>();

    public List<int> GetScore()
    {
        return _score;
    }

    public string GetScoreString()
    {
        string scoreStr = "";

        int number = 1;

        for (int i = _score.Count - 1; i >= 0; i--)
        {
            scoreStr += number.ToString() + ". " + _score[i].ToString() + "\n";
            number++;
        }

        return scoreStr;
    }

    public void UpdateScore(int points)
    {
        AddNote(points);
    }

    private void AddNote(int points)
    {
        if (_score.Count != 0)
        {
            int currentMax = _score[_score.Count - 1];
            if (points > currentMax)
            {
                if (_score.Count >= maximumNotes)
                {
                    _score.RemoveAt(0);
                }
                _score.Add(points);
            }
        }
        else
        {
            _score.Add(points);
        }
    }

    public void ResetScore()
    {
        _score.Clear();
    }
}
