using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    InGame,
    GameEnd
}

public class GameManager : MonoBehaviour
{
    public SimpleRuntimeUI inGameUi;

    private ScoreManager scoreManager;
    private GameState gameState;

    public float gameTimeLimit = 60.0f;
    public float gameTimeLowWarning = 10.0f;

    float elapsedTime = 0.0f;
    bool timeLowSet = false;

    private void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
        if (scoreManager == null)
        {
            throw new MissingComponentException("Score manager not found in the scene!");
        }
    }

    void Update()
    {
        if (gameState == GameState.InGame) 
        {
            if (elapsedTime < gameTimeLimit)
            {
                elapsedTime += Time.deltaTime;
                inGameUi.SetRemainingTimeSeconds(gameTimeLimit - elapsedTime);

                if (gameTimeLimit - elapsedTime <= gameTimeLowWarning && !timeLowSet) 
                {
                    inGameUi.SetRemainingTimeLow(true);
                    timeLowSet = true;
                }

            }
            else 
            {
                WinGame();
            }
        }
    }

    public void ResetElapsedTime()
    {
        elapsedTime = 0.0f;
        inGameUi.SetRemainingTimeLow(false);
        timeLowSet = false;
    }

    public void RestartGame()
    {
        gameState = GameState.InGame;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EnterMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void FinishGame() // Game failed - hit a bulding or smth
    {
        gameState = GameState.GameEnd;
        inGameUi.ToggleEndScreen(true);
    }
    public void WinGame() // Game won - timer finished
    {
        gameState = GameState.GameEnd;
        inGameUi.ToggleWinScreen(true);
        Time.timeScale = 0;
        // TODO: Finishes the game, displays high score
    }

    public GameState GetGameState()
    {
        return gameState;
    }
}
