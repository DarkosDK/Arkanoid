using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{

    [Header("Slow Power Up")]
    [Range(1, 10)]
    public float slowDuration;
    
    [Range(0, 1)]
    public float slowPower;
    
    [Header("Stretch Power Up")]
    [Range(1, 10)]
    public float stretchDuration;
    
    [Range(0.5f, 2)]
    public float stretchFactor;

    private Platform _platform;
    private Ball _ball;

    // Start is called before the first frame update
    void Start()
    {
        _platform = GameObject.FindWithTag("Platform").GetComponent<Platform>();
    }
    
    //Apply power up behavior
    public void ApplyPowerUp(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.Slow:
                SlowBall();
                break;
            
            case PowerUpType.Stretch:
                StretchPlatform();
                break;
        }
    }

    private void SlowBall()
    {
        _ball = GameObject.FindGameObjectWithTag("Ball")?.GetComponent<Ball>();
        if (_ball != null)
        {
            StartCoroutine(SlowCountdownRoutine());
        }
    }

    private void StretchPlatform()
    {
        StartCoroutine(StretchCountdownRoutine());
    }

    IEnumerator SlowCountdownRoutine()
    {
        float initSpeed = _ball.GetSpeed();
        _ball.SetSpeed(initSpeed * (1 - slowPower));
        yield return new WaitForSeconds(slowDuration);
        //_ball.SetSpeed(initSpeed);

        if (_ball ?? null)
        {
            float newSpeed = _ball.GetSpeed();
            _ball.SetSpeed(newSpeed * (1/(1 - slowPower)));
        }
        
    }

    IEnumerator StretchCountdownRoutine()
    {
        _platform.StretchLength(stretchFactor);
        yield return new WaitForSeconds(stretchDuration);
        _platform.StretchLength(1/stretchFactor);
    }
}
