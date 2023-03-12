using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public GameplayManager gameplayManager;

    /// <summary>
    /// generates a random position for the apple after it was collected
    /// </summary>
    public void GenerateRandomPosition()
    {
        transform.position = gameplayManager.GenerateCoordinatesForSpawning();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Snake"))
        {
            GenerateRandomPosition();
        }
    }
}
