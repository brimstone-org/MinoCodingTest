using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public GameplayManager gameplayManager;

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
