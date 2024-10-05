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
    float elapsedTime = 0.0f;

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
            }
            else 
            {
                FinishGame();
            }
        }
    }

    public void ResetElapsedTime()
    {
        elapsedTime = 0.0f;
    }

    public void EnterGame()
    {
        gameState = GameState.InGame;
        inGameUi.gameObject.SetActive(true);
        SceneManager.LoadScene("SimplePoly City - Low Poly Assets_Demo Scene");
        Debug.Log("Sport");
    }

    public void RestartGame()
    {
        gameState = GameState.InGame;
        inGameUi.ToggleEndScreen(false);
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
