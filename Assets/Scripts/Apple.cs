using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public GameplayManager gameplayManager;

    private void Start()
    {
        GenerateRandomPosition();
    }

    public void GenerateRandomPosition()
    {
        transform.position = gameplayManager.GenerateCoordinatesForFood();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Snake"))
        {
            GenerateRandomPosition();
        }
    }
}
