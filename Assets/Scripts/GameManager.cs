using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverDialog;
    public GameObject pauseDialog;
    public LevelGenerator levelGenerator;
    
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private static GameManager _instance = null;

    [SerializeField] private Ball ballPrefab;

    private bool _isGameActive = false;
    private bool _isGameOver = false;
    private bool _isPause = false;

    private int _brickCount;
    
    private int _life = 3;
    private int _points = 0;
    
    private Ball _ball;
    private Rigidbody2D _ballRB;
    private Platform _platform;

    public UnityEvent gameOverEvent;
    public UnityEvent startGameEvent;
    public PointRaiseEvent pointRaiseEvent;
    public ScoreUpdateEvent scoreUpdateEvent;
    public UnityEvent updatePowerUps;
    
    private void Awake()
    {
        if (_instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this;
    }

    void Start()
    {
        _platform = GameObject.FindGameObjectWithTag("Platform").GetComponent<Platform>();
        StartGame();
    }

    private void Update()
    {
        // Run ball when LMB pressed
        if (Input.GetMouseButtonDown(0) && !_isGameOver && !_isGameActive)
        {
            _isGameActive = true;
            _ballRB.isKinematic = false;
            _ballRB.velocity = new Vector2(0, _ball.GetSpeed());
        }
    }

    void FixedUpdate()
    {
        // Attach ball to platform center before run the game
        if (!_isGameActive && !_isGameOver)
        {
            Vector3 platformPos = _platform.transform.position;

            Transform ball = _ball.transform;
            Vector3 ballPos = ball.position;
            ball.position = new Vector3(platformPos.x, ballPos.y, 0);
        }
    }

    private void StartGame()
    {
        // Turn off UI elements
        gameOverDialog.SetActive(false);
        pauseDialog.SetActive(false);
        
        InitBall();
    }

    public void GameOver()
    {
        //Turn on Ui "Game OVER"
        gameOverDialog.SetActive(true);
        
        scoreUpdateEvent.Invoke(_points);
        gameOverEvent.Invoke();
        
        //Turn off ball
        _ball.gameObject.SetActive(false);
        
        _isGameActive = false;
        _isGameOver = true;
    }

    private void InitBall()
    {
        // Delete old balls
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }
        
        //Add new ball
        Vector2 platformPos = _platform.transform.position;
        Vector2 ballPos = new Vector2(platformPos.x, platformPos.y + 0.37f);
        _ball = Instantiate(ballPrefab, ballPos, Quaternion.identity);
        _ballRB = _ball.GetComponent<Rigidbody2D>();

        _isGameActive = false;
        
        // Destroy power ups, if needed
        updatePowerUps.Invoke();
    }

    // Get all brick count on current level
    private int GetBrickCount()
    {
        return GameObject.FindGameObjectsWithTag("Brick").Length;
    }
    
    public void InitBrickCount()
    {
        _brickCount = GetBrickCount();
    }

    public void DecreaseBricks()
    {
        _brickCount -= 1;
        if (_brickCount <= 0)
        {
            if (!_isGameOver)
            {
                NextLevel();
            }
        }
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1;
        _isGameOver = false;
        _platform.gameObject.SetActive(true);

        InitBall();
        levelGenerator.ResetLevel();
        levelGenerator.GenerateLevel();
        InitBrickCount();
        
        //Reset Points
        _points = 0;
        AddPoint(0);
        
        //Reset life
        _life = 3;
        
        startGameEvent.Invoke();
    }

    
    private void NextLevel()
    {
        levelGenerator.LevelUp();
        levelGenerator.GenerateLevel();
        InitBall();
        InitBrickCount();
    }

    public void BallLose()
    {
        _life--;
        if (_life <= 0)
        {
            GameOver();
        }
        else
        {
            InitBall();
        }
    }

    public void AddPoint(int value)
    {
        _points += value;
        pointRaiseEvent.Invoke(_points);
    }

    public void PauseGame()
    {
        if (_isPause)
        {
            Time.timeScale = 1;
            _isPause = false;
        }
        else
        {
            Time.timeScale = 0;
            _isPause = true;
            
        }
        pauseDialog.SetActive(_isPause);
    }

    public bool GetPause()
    {
        return _isPause;
    }
    
    //Exit Game for buttons
    public void GameExit()
    {
        Application.Quit();
    }
}

[System.Serializable]
public class PointRaiseEvent : UnityEvent<int> { };

[System.Serializable]
public class ScoreUpdateEvent : UnityEvent<int> { };
