using System;
namespace DrillDown;

public class GameManager
{
    public enum GameState
    {
        MainMenu,
        Playing
    }

    public event Action OnGameStart;
    public GameState gameState { get; private set; } = GameState.MainMenu;

    public GameManager()
    {

    }

    public void StartGame()
    {
        gameState = GameState.Playing;
        OnGameStart?.Invoke();
    }
}