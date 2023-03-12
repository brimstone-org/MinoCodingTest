using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask gamePlayLayer;
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
    private Apple applePrefab; // the apple prefab to spawn
    private Apple apple; //the apple object in the game
    //the below variables represent the ratio between the default camera orthographic size and the optimal sizes of the board on both axis. They are used to zoom the camera out or in based on the size of the board
    private float cameraXRatio = 3f;
    private float cameraYRatio = 1.5f;
    private UIUXManager uiuxManager; //uimanager gameplay
    private int difficultyLevel = 1; //easy by default
    private int numberOfSnakes = 0; //number of snakes in the game

    private void Awake()
    {
        uiuxManager = FindObjectOfType<UIUXManager>();
    }


    private void Start()
    {
       
        //Due to having collider issues because of small colliders I increased the scale of the whole game time 4. Here, I am adjusting the grid to have sizes that are multiples of 4
        var outputWidth = GetNearestWholeMultiple(gridWidth,4);
        var outputHeight = GetNearestWholeMultiple(gridHeight, 4);
        //set board scale
        gridBackground.localScale = new Vector3(outputWidth, outputHeight, 0);
        //set borders positions
        topBorder.localPosition = new Vector2(0, Mathf.CeilToInt((float)outputHeight / 8)*4);
        bottomBorder.localPosition = new Vector2(0, -Mathf.CeilToInt((float) outputHeight / 8)*4);
        rightBorder.localPosition = new Vector2(Mathf.CeilToInt((float)outputWidth / 8) *4 , 0 );
        leftBorder.localPosition = new Vector2(-Mathf.CeilToInt((float) outputWidth / 8)*4 , 0);
        //set borders scales
        topBorder.localScale = new Vector3(outputWidth - (outputWidth%2)+4, 4, 0);
        bottomBorder.localScale = new Vector3(outputWidth - (outputWidth % 2)+4  , 4, 0);
        rightBorder.localScale = new Vector3(4, outputHeight- (outputHeight % 2)+4 , 0);
        leftBorder.localScale = new Vector3(4, outputHeight- (outputHeight % 2)+4, 0);
        //set camera size
        if (outputWidth >= outputHeight)
        {
            Camera.main.orthographicSize = outputWidth / cameraXRatio;
            if ((outputWidth/outputHeight) <= cameraYRatio*1.1f)
            {
                Camera.main.orthographicSize = outputHeight / cameraYRatio;
            }
        }
        else
        {
            Camera.main.orthographicSize = outputHeight / cameraYRatio;
        }
        SelectDifficultyLevel(difficultyLevel);
    }

    /// <summary>
    /// get proper value based on the game multiplier. I had to multiply everything by 4 to solve the issue of colliders not working
    /// </summary>
    /// <param name="input"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    private int GetNearestWholeMultiple(int input, int x)
    {
        int output = Mathf.RoundToInt(input / x);
        if (output == 0 && input > 0) output += 1;
        output *= x;

        return output;
    }

    /// <summary>
    /// generates the new coordinates for the apple and the snakes
    /// </summary>
    /// <returns></returns>
    public Vector2 GenerateCoordinatesForSpawning()
    {
        int x = GetNearestWholeMultiple((int)Random.Range(leftBorder.position.x+4 ,
                              rightBorder.position.x-4 ), 4);

        // y position between top & bottom border
        int y = GetNearestWholeMultiple((int)Random.Range(bottomBorder.position.y+4,
                                  topBorder.position.y-4 ), 4);
        //check if another object is in the way
        if (Physics2D.OverlapCircle(new Vector2(x, y), 1f, gamePlayLayer)!=null)
        {
            return GenerateCoordinatesForSpawning();
        }
        else
        {
            return new Vector2(x, y);
        }
       
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
                Time.fixedDeltaTime = 0.09f;
                break;
            case 3:
                Time.fixedDeltaTime = 0.07f;
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
    /// removes a snake from the game
    /// </summary>
    public void RemoveSnake()
    {
        numberOfSnakes--;
        if (numberOfSnakes <= 0)
        {
            //game over
            if (apple != null)
            {
                Destroy(apple.gameObject);
            }
            uiuxManager.ToggleMenu();
        }
    }

    /// <summary>
    /// Generates snakes and starts the game
    /// </summary>
    public void StartTheGame()
    {
        for (int i = 0; i < numberOfSnakes; i++)
        {
            Snake newSnake = Instantiate(snakePrefab, GenerateCoordinatesForSpawning(), Quaternion.identity);
            newSnake.gameplayManager = this;
            newSnake.uiuxManager = uiuxManager;
            uiuxManager.SpawneScore(i);
            newSnake.InitializeSnake(i);
        }
        apple = Instantiate(applePrefab, GenerateCoordinatesForSpawning(), Quaternion.identity);
        apple.gameplayManager = this;
    }
    
}
