
using System.Drawing;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace DrillDown;

public class MineralsShop : Shop
{
    private Text totalValueText;
    public MineralsShop(string spriteName, float scale, float worldXPos, Player player) : base(spriteName, scale, worldXPos, player)
    {
        panel = new Panel(new Sprite("Panel1"), 3, 3);
        panel.SetTitle("MineralsShop");
        panel.AddButton(7,"Sell All",() => SellAll());
        panel.AddCloseButton(2,CloseShop);
        totalValueText = new Text
        {
            font = Game1._font,
            color = Color.White,
            sortingOrder = 1f
        };
        totalValueText.font = Game1._font;
        totalValueText.tm.position = panel.GetSlotCenter(4);
    }

    private void SellAll()
    {
        if (player == null) return;
        int totalValue = player.Inventory.GetOresTotalValue();
        player.AddMoney(totalValue);
        player.Inventory.ClearInventory();
    }
    
    public override void DrawPanel(SpriteBatch spriteBatch)
    {
        base.DrawPanel(spriteBatch);
        if (!isOpen) return;
        totalValueText.text = $"Total Price: $ {player.Inventory.GetOresTotalValue()}";
        totalValueText.DrawTextBackground(spriteBatch);
        totalValueText.Draw(spriteBatch);
    }
}