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
        var isNumeric = int.TryParse("123", out int numberOfSnakes);
        if (string.IsNullOrEmpty(noOfPlayersInput.text) || string.IsNullOrWhiteSpace(noOfPlayersInput.text) || !isNumeric || numberOfSnakes>4)
        {
            return;
        }
        gameplayManager.SetNumberOfSnakes(numberOfSnakes);
        gameplayManager.StartTheGame();
        mainMenu.SetActive(false);
        
    }

}
