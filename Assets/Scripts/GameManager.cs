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
        DontDestroyOnLoad(gameObject);
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

    public void EnterGame()
    {
        gameState = GameState.InGame;
        inGameUi.gameObject.SetActive(true);
        inGameUi.HideAllOverlays();
        ResetElapsedTime();
        SceneManager.LoadScene("SimplePoly City - Low Poly Assets_Demo Scene");
        Debug.Log("Sport");
    }

    public void RestartGame()
    {
        gameState = GameState.InGame;
        inGameUi.HideAllOverlays();
        scoreManager.ResetScore();
        ResetElapsedTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EnterMenu()
    {
        gameState = GameState.Menu;
        inGameUi.gameObject.SetActive(false);
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
        // TODO: Finishes the game, displays high score
    }

    public GameState GetGameState()
    {
        return gameState;
    }
}
