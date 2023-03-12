using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUXManager : MonoBehaviour
{
    private GameplayManager gameplayManager; //gameplay manager reference
    [SerializeField]
    private InputField noOfPlayersInput;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private Transform scoreTransform; //the holder where we'll spawn the 
    [SerializeField]
    private GameObject scorePrefab; //prefab for the score item
    private List<Text> scores = new List<Text>(); //list of score items

    private void Awake()
    {
        gameplayManager = FindObjectOfType<GameplayManager>();
    }

    /// <summary>
    /// attached to each toggle on the UI and used to set the current difficulty
    /// </summary>
    /// <param name="difficulty"></param>
    public void SelectDifficultyLevel(int difficulty)
    {
        gameplayManager.SelectDifficultyLevel(difficulty);
    }


    //Calls the start game function in the gameplay manager
    public void StartTheGame()
    {
        var isNumeric = int.TryParse(noOfPlayersInput.text, out int numberOfSnakes);
        if (string.IsNullOrEmpty(noOfPlayersInput.text) || string.IsNullOrWhiteSpace(noOfPlayersInput.text) || !isNumeric || numberOfSnakes>4)
        {
            return;
        }
        //clear score board
        foreach (Transform child in scoreTransform)
        {
            Destroy(child.gameObject);
        }
        scores.Clear();
        gameplayManager.SetNumberOfSnakes(numberOfSnakes);
        gameplayManager.StartTheGame();
        ToggleMenu();
    }

    /// <summary>
    /// toggles the main menu
    /// </summary>
    public void ToggleMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
    }

    /// <summary>
    /// spawns a score element
    /// </summary>
    /// <param name="index"></param>
    public void SpawneScore(int index)
    {
        GameObject newScore = Instantiate(scorePrefab, scoreTransform);
        Text newText = newScore.GetComponent<Text>();
        newText.text = "Player "+(index+1)+ ": 0";
        scores.Add(newText);
    }

    /// <summary>
    /// updates the score for a snake
    /// </summary>
    /// <param name="index"></param>
    /// <param name="score"></param>
    public void UpdateScore(int index, int score)
    {
        scores[index].text = "Player " + (index+1) + ": " + score;
    }

}
