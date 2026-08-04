using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class GameOver : IDrawable,IUpdatable
{
    private Panel gameOverPanel;
    private Text finishText; 
    private string finishT =
        "You broke through the last stone and found the portal.\nWaiting in the dark...\n" +
        "This world is empty now...\nIts gold and diamond ride in your hold.\n" +
        "You aim the pod at the light and burn the last of your fuel.\n" +
        "The planet fades.\nAnother one waits somewhere ahead, deeper and richer.\n" +
        "\n                   NEXT PLANET COMING SOON...";

    public GameOver()
    {
        gameOverPanel = new Panel(new Sprite("Panel1"), 3, 5,2f,0);
        gameOverPanel.SetTitle("Game Over");
        finishText = Text.CreateDefault(finishT);
        finishText.tm.position = gameOverPanel.GetPanelCenter() + new Vector2(0,-100f);
        //gameOverPanel.AddText(3,finishT,1.1f);
        //gameOverPanel.AddButton(4,"Start",() => GameManager.Instance.StartGame());
        //gameOverPanel.AddButton(7,"Settings",() => Console.WriteLine("Settings"));
        gameOverPanel.AddButton(10,"KeepPlaying",() => GameManager.Instance.StartGame());
        gameOverPanel.AddButton(13,"Exit",() => GameManager.Instance.QuitGame());
    }
    
    public void Start()
    {
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        gameOverPanel.DrawPanel(spriteBatch);
        finishText.DrawTextBackground(spriteBatch);
        finishText.Draw(spriteBatch);
    }
    
    public void Update(GameTime gameTime)
    {
        gameOverPanel.UpdatePanel(gameTime);
    }
}