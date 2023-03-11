using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private int gridWidth = 48;
    [SerializeField]
    private int gridHeight = 24;
    [SerializeField]
    private Transform gridBackground;
    [SerializeField]
    private Transform topBorder;
    [SerializeField]
    private Transform bottomBorder;
    [SerializeField]
    private Transform rightBorder;
    [SerializeField]
    private Transform leftBorder;
    [SerializeField]
    private Snake snakePrefab;
    [SerializeField]
    private Apple applePrefab;
    //the below variables represent the ratio between the default camera orthographic size and the optimal sizes of the board on both axis. They are used to zoom the camera out or in based on the size of the board
    private float cameraXRatio = 3.2f;
    private float cameraYRatio = 1.6f;
    private UIUXManager uiuxManager; //uimanager gameplay
    private int difficultyLevel = 1; //easy by default
    private int numberOfSnakes = 0; //easy by default

    private void Awake()
    {
        uiuxManager = FindObjectOfType<UIUXManager>();

    }


    private void Start()
    {
        //set board scale 
        gridBackground.localScale = new Vector3(gridWidth, gridHeight, 0);
        //set borders positions
        topBorder.localPosition = new Vector2(0, Mathf.Round(gridHeight / 2));
        bottomBorder.localPosition = new Vector2(0, Mathf.Round(-gridHeight / 2));
        rightBorder.localPosition = new Vector2(Mathf.Round(gridWidth / 2), 0 );
        leftBorder.localPosition = new Vector2(Mathf.Round(-gridWidth / 2), 0);
        //set borders scales
        topBorder.localScale = new Vector3(gridWidth - (gridWidth%2-1), 1, 0);
        bottomBorder.localScale = new Vector3(gridWidth - (gridWidth % 2-1), 1, 0);
        rightBorder.localScale = new Vector3(1, gridHeight- (gridHeight % 2-1), 0);
        leftBorder.localScale = new Vector3(1, gridHeight- (gridHeight % 2-1), 0);
        //set camera size
        if (gridWidth >= gridHeight)
        {
            Camera.main.orthographicSize = gridWidth / cameraXRatio;
        }
        else
        {
            Camera.main.orthographicSize = gridHeight / cameraYRatio;
        }
        SelectDifficultyLevel(difficultyLevel);
    }

    /// <summary>
    /// generates the new coordinates for the apple and the snakes
    /// </summary>
    /// <returns></returns>
    public Vector2 GenerateCoordinatesForSpawning()
    {
        int x = (int)Random.Range(leftBorder.position.x,
                              rightBorder.position.x);

        // y position between top & bottom border
        int y = (int)Random.Range(bottomBorder.position.y,
                                  topBorder.position.y);
        return new Vector2(x, y);
    }

    /// <summary>
    /// chooses the difficulty level and the game's fixed timestamp
    /// </summary>
    /// <param name="difficulty"></param>
    public void SelectDifficultyLevel(int difficulty)
    {
        difficultyLevel = difficulty;
        switch (difficultyLevel)
        {
            case 1:
                Time.fixedDeltaTime = 0.12f;
                break;
            case 2:
                Time.fixedDeltaTime = 0.08f;
                break;
            case 3:
                Time.fixedDeltaTime = 0.04f;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Sets the number of players for the game
    /// </summary>
    /// <param name="snakes"></param>
    public void SetNumberOfSnakes(int snakes)
    {
        numberOfSnakes = snakes;
    }

    /// <summary>
    /// Generates snakes and starts the game
    /// </summary>
    public void StartTheGame()
    {
        for (int i = 0; i < numberOfSnakes; i++)
        {
            Snake newSnake = Instantiate(snakePrefab, GenerateCoordinatesForSpawning(), Quaternion.identity);
            newSnake.InitializeSnake(i);
        }
        Apple apple = Instantiate(applePrefab, GenerateCoordinatesForSpawning(), Quaternion.identity);
        apple.gameplayManager = this;
    }
    
}
