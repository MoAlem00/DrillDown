using System;
namespace DrillDown;

public class GameManager
{
    private static GameManager instance = null;
    public enum GameState
    {
        MainMenu,
        GameOver,
        WinGame,
        Playing
    }

    public event Action OnGameStart;
    public event Action OnGameRestart;
    //public event Action OnGameWon;
    public event Action OnQuitGame;
    public GameState gameState { get; private set; } = GameState.MainMenu;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }

            return instance;
        }
    }

    public void StartGame()
    {
        gameState = GameState.Playing;
        OnGameStart?.Invoke();
    }
    public void QuitGame() => OnQuitGame?.Invoke();

    public void HandleGameOver()
    {
        gameState = GameState.GameOver;
    }
    
    public void WinGame()
    {
        gameState = GameState.WinGame;
    }

    public void RestartGame()
    {
        OnGameRestart?.Invoke();
        gameState = GameState.Playing;
    }
}