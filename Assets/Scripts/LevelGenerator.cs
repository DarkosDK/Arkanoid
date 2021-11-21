using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    //Power ups variations 
    [SerializeField] private GameObject[] powerUps = new GameObject[2];
    
    // Bricks with power ups in the percentage of all bricks count
    [Range(0, 100)]
    public int powerUpPercentage;
    
    // Max levels rows and columns
    private int _rowCount = 13;
    private int _columnCount = 8;

    private List<Brick> _bricks = new List<Brick>();

    [SerializeField] private int _currentLevel;

    private int _initRows = 4; //Start rows count

    private int _initColor = 0; //First row color index

    private Material _defaultMat;

    private void Start()
    {
        //Collect all bricks
        foreach (Transform obj in transform)
        {
            _bricks.Add(obj.GetComponent<Brick>());
        }

        //Default material for bricks (without power ups)
        _defaultMat = transform.GetChild(0).GetComponent<SpriteRenderer>().material;
        
        
        _currentLevel = 0;
        
        GenerateLevel();
        GameManager.Instance.InitBrickCount();
    }

    private void ActivateBrick(int ind)
    {
        _bricks[ind].gameObject.SetActive(true);
    }

    //Disable all bricks
    private void ResetBricks()
    {
        foreach (Brick brick in _bricks)
        {
            if (_currentLevel == 0)
            {
                brick.ResetHealth();
            }
            brick.gameObject.SetActive(false);
        }
    }

    private void ResetPowerUps()
    {
        foreach (Brick brick in _bricks)
        {
            brick.powerUpPrefab = null;
            brick.gameObject.GetComponent<SpriteRenderer>().material = _defaultMat;
        } 
    }

    public void GenerateLevel()
    {
        ResetBricks();
        
        // Brick count for current layer to fill on
        int activeBricksCount = 0;
        
        // Add one row every 2 levels
        int currentRowCount = _initRows + (_currentLevel / 2);
        
        for (int row = 0; row < _rowCount; row++)
        {
            int rowColor = (row + _initColor) % 4;
            for (int col = 0; col < _columnCount; col++)
            {
                int ind = row * _columnCount + col;
                Brick brick = _bricks[ind];
                
                // Start increase health by 1 on the top rows
                if (row < _currentLevel)
                {
                    brick.IncreaseHealth();
                }

                if (row < currentRowCount)
                {
                    // Set different brick types depending on row
                    brick._brickType = (BrickType) rowColor;
                    brick.brickObject = brick.SetBrickTypeEditor();
                    brick.SetBrickTexture();
                    
                    ActivateBrick(ind);
                    activeBricksCount++;
                }
            }
        }
        
        //Add Random power up
        ResetPowerUps();
        
        int powerUpsCount = (int)(activeBricksCount * (powerUpPercentage / 100.0f));
        
        foreach (int ind in GenerateRandomNumbers(powerUpsCount, activeBricksCount))
        {
            int randomPowerUpInd = Random.Range(0, powerUps.Length);
            GameObject powerUpChose = powerUps[randomPowerUpInd];
            _bricks[ind].powerUpPrefab = powerUpChose;

            Material effectMat = powerUpChose.GetComponent<PowerUp>().effectMaterial;
            _bricks[ind].GetComponent<SpriteRenderer>().material = effectMat;
        }
    }
    
    // Generate random indexes from all bricks indexes to apply power up
    private int[] GenerateRandomNumbers(int countNumbers, int countRange)
    {
        int[] outNumbers = new int[countNumbers];
        
        List<int> numbersForGenerate = new List<int>();
        for (int i = 0; i < countRange; i++)
        {
            numbersForGenerate.Add(i);
        }

        for (int i = 0; i < countNumbers; i++)
        {
            int randomInd = Random.Range(0, numbersForGenerate.Count);
            outNumbers[i] = numbersForGenerate[randomInd];
            
            numbersForGenerate.RemoveAt(randomInd);
        }

        return outNumbers;
    }

    public void LevelUp()
    {
        _currentLevel++;
    }

    public void ResetLevel()
    {
        _currentLevel = 0;
    }
}
