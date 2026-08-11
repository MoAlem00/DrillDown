using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class GameOverMenu : Menu
{

    public GameOverMenu()
    {
        panel = new Panel(new Sprite("Panel1"), 3, 5,1f,0);
        panel.SetTitle("Game Over");
        panel.AddButton(7,"Restart",() => GameManager.Instance.RestartGame(),180,70);
        panel.AddButton(10,"Exit",() => GameManager.Instance.QuitGame(),180,70);
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        panel.DrawPanel(spriteBatch);
    }
}