using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    InGame,
    GameEnd
}

public class GameManager : MonoBehaviour
{
    public SimpleRuntimeUI inGameUi;

    public ScoreManager scoreManager;

    private GameState gameState;

    public float gameTimeLimit = 60.0f;
    public float gameTimeLowWarning = 10.0f;

    [Header("Audio")]
    public AudioSource tickTockAudioSource;
    public AudioClip winSound;

    [Range(0.0f, 1.0f)]
    public float tickTockStart = 0.5f;
    
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
        if (gameState == GameState.InGame && Input.GetKeyDown(KeyCode.Escape))
        {
            inGameUi.TogglePauseScreen();
        }


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
                UpdateTickTockSound();
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
        tickTockAudioSource.volume = 0.0f;
    }
    public void WinGame() // Game won - timer finished
    {
        gameState = GameState.GameEnd;
        inGameUi.OpenWinOverlay(scoreManager.GetScore());
        Time.timeScale = 0;
        tickTockAudioSource.volume = 0.0f;
        AudioManager.PlayAudioClip(winSound, transform, 0.5f);
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    private void UpdateTickTockSound()
    {
        float gameProgress = elapsedTime / gameTimeLimit;
        if (gameProgress > tickTockStart)
        {
            tickTockAudioSource.volume = (gameProgress - tickTockStart) * 1.0f / (1.0f - tickTockStart);
        }
    }
}
