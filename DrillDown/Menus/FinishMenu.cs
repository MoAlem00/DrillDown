using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class FinishMenu : Menu
{
    private Text finishText; 
    private string finishT =
        "You broke through the last stone and found the portal.\nWaiting in the dark...\n" +
        "This world is empty now...\nIts gold and diamond ride in your hold.\n" +
        "You aim the pod at the light and burn the last of your fuel.\n" +
        "The planet fades.\nAnother one waits somewhere ahead, deeper and richer.\n" +
        "\n                   NEXT PLANET COMING SOON...";


    public FinishMenu()
    {
        panel = new Panel(new Sprite("Panel1"), 3, 5,2f,0);
        panel.SetTitle("Game Finished");
        finishText = Text.CreateDefault(finishT);
        finishText.tm.position = panel.GetPanelCenter() + new Vector2(0,-100f);
        panel.AddButton(10,"KeepPlaying",() => GameManager.Instance.StartGame());
        panel.AddButton(13,"Exit",() => GameManager.Instance.QuitGame());
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        panel.DrawPanel(spriteBatch);
        finishText.DrawTextBackground(spriteBatch);
        finishText.Draw(spriteBatch);
    }
    
    
}