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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void EnterGame()
    {
        gameState = GameState.InGame;
        inGameUi.gameObject.SetActive(true);
        SceneManager.LoadScene("SimplePoly City - Low Poly Assets_Demo Scene");
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
        inGameUi.gameObject.SetActive(false);
        SceneManager.LoadScene("Menu");
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
