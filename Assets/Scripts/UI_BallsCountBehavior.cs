using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BallsCountBehavior : MonoBehaviour
{
    private List<GameObject> ballsCount = new List<GameObject>();

    private int _currentBallsCount;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            ballsCount.Add(child.gameObject);
            _currentBallsCount = ballsCount.Count;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecreaseBallsCount()
    {
        ballsCount[_currentBallsCount - 1].SetActive(false);
        _currentBallsCount--;
    }

    public void InitBallsCount()
    {
        _currentBallsCount = ballsCount.Count;
        foreach (GameObject ball in ballsCount)
        {
            ball.SetActive(true);
        }
        
    }
}
