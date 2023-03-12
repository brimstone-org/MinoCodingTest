using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    [Header("Snake Data")]
    [SerializeField]
    private List<Transform> snakeSegments = new List<Transform>();
    [SerializeField]
    private Transform snakeSegmentPrefab;
    private Color32 snakeColor = new Color32();
    private int snakeIndex = 0;
    private int applesCollected = 0;
    
    [Header("References to Managers")]
    public GameplayManager gameplayManager; //reference to the gameplay manager
    public UIUXManager uiuxManager; //reference to the ui manager

    [Header("Input and Movement")]
    private Vector2 direction = Vector2.zero; //vector to keep track of which direction the snake is moving in
    [SerializeField]
    private List<Color32> snakeColors = new List<Color32>();
    private SnakeController snakeController;
    private InputAction move; //stores a reference to the move action

    /// <summary>
    /// called by the gameplay manager after spawning the snake
    /// </summary>
    public void InitializeSnake(int index)
    {
        snakeSegments.Add(transform);
        snakeController = new SnakeController(); //initializes the player input
        move = snakeController.FindAction("MovePlayer" + (index + 1));
        move.performed += DoSetMoveDirection;
        snakeColor = snakeColors[index];
        GetComponent<SpriteRenderer>().color = snakeColor;
        snakeIndex = index;
        snakeController.Enable();
    }

    
    /// <summary>
    /// adds a new segment to the snake
    /// </summary>
    public void ExtendSnake()
    {
        Transform newSegment = Instantiate(snakeSegmentPrefab);
        newSegment.position = snakeSegments[snakeSegments.Count - 1].position;
        newSegment.GetComponent<SpriteRenderer>().color = snakeColor;
        applesCollected++;
        uiuxManager.UpdateScore(snakeIndex, applesCollected);
        snakeSegments.Add(newSegment);
        
    }

    //called when the snake dies
    private void DestroySnake()
    {
        for (int i = snakeSegments.Count-1; i >0; i--)
        {
            Destroy(snakeSegments[i].gameObject);
            snakeSegments.RemoveAt(i);
        }
        snakeSegments.TrimExcess();
        move.performed -= DoSetMoveDirection;
        snakeController.Disable();
        gameplayManager.RemoveSnake();
        Destroy(gameObject);
    }

    /// <summary>
    /// called by the bindings in the input system
    /// </summary>
    /// <param name="context"></param>
    private void DoSetMoveDirection(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().normalized.y>0)
        {
            if (direction!=Vector2.down)
                direction = Vector2.up;
        }
        else if (context.ReadValue<Vector2>().normalized.y< 0)
        {
            if (direction != Vector2.up)
                direction = Vector2.down;
        }
        else if (context.ReadValue<Vector2>().normalized.x < 0)
        {
            if (direction != Vector2.right)
                direction = Vector2.left;
        }
        else if (context.ReadValue<Vector2>().normalized.x > 0)
        {
            if (direction != Vector2.left)
                direction = Vector2.right;
        }
        ////if (direction.normalized.x != -newDir.normalized.x && direction.normalized.y != -newDir.normalized.y)
        //{
        //    direction = context.ReadValue<Vector2>().normalized;
        //}
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Apple"))
        {
            ExtendSnake();
        }
        else //collided with anything else
        {
            DestroySnake();
        }
    }


    /// <summary>
    /// moves the snake
    /// </summary>
    private void FixedUpdate()
    {
        for (int i = snakeSegments.Count-1; i >0; i--)
        {
            snakeSegments[i].position = snakeSegments[i - 1].position;
        }
        transform.position = new Vector2(Mathf.Round(transform.position.x) + direction.x, Mathf.Round(transform.position.y) + direction.y);
    }
}
