using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 direction = Vector2.zero; //vector to keep track of which direction the snake is moving in
    [SerializeField]
    private List<Transform> snakeSegments = new List<Transform>();
    [SerializeField]
    private Transform snakeSegmentPrefab;
    
    public GameplayManager gameplayManager; //reference to the gameplay manager
    public UIUXManager uiuxManager; //reference to the ui manager

    private void Start()
    {
        snakeSegments.Add(transform);
    }

    /// <summary>
    /// adds a new segment to the snake
    /// </summary>
    public void ExtendSnake()
    {
        Transform newSegment = Instantiate(snakeSegmentPrefab);
        newSegment.position = snakeSegments[snakeSegments.Count - 1].position;
        snakeSegments.Add(newSegment);
    }

    private void ResetSnake()
    {
        for (int i = snakeSegments.Count-1; i >0; i--)
        {
            Destroy(snakeSegments[i].gameObject);
            snakeSegments.RemoveAt(i);
        }
        snakeSegments.TrimExcess();
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Apple"))
        {
            ExtendSnake();
        }
        else //collided with anything else
        {
            ResetSnake();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector2.right;
        }
    }


    private void FixedUpdate()
    {
        for (int i = snakeSegments.Count-1; i >0; i--)
        {
            snakeSegments[i].position = snakeSegments[i - 1].position;
        }
        transform.position = new Vector2(Mathf.Round(transform.position.x) + direction.x, Mathf.Round(transform.position.y) + direction.y);
    }
}
