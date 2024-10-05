using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Menu,
    InGame,
    GameEnd
}

public class GameManager : MonoBehaviour
{
    public SimpleRuntimeUI inGameUi;

    private GameState gameState;


    public void EnterGame()
    {
        gameState = GameState.InGame;
        // TODO: Loads the game level from the menu
    }

    public void RestartGame()
    {
        gameState = GameState.InGame;
        inGameUi.ToggleEndScreen(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EnterMenu()
    {
        gameState = GameState.Menu;
        // TODO: changes the scene to the menu level
    }

    public void FinishGame()
    {
        gameState = GameState.GameEnd;
        inGameUi.ToggleEndScreen(true);
        // TODO: Finishes the game, displays high score
    }

    public GameState GetGameState()
    {
        return gameState;
    }
}
