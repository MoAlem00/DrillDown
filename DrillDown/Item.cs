using System;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public enum ItemType{Bomb,Teleport,FuelTank,RepairKit}
public abstract class Item : IUsable//,IDrawable
{
    private ItemType type;
    private Sprite icon;
    private int cost;
    private int quantity;

    public ItemType Type => type;
    public int Cost => cost;
    public Sprite Icon => icon;
    public int Quantity => quantity;

    public Item(ItemType type, Sprite icon, int cost)
    {
        this.type = type;
        this.icon = icon;
        this.cost = cost;
    }
    
    public abstract void Use(Player player);
    public void AddQuantity(int amount) => quantity += amount;
    /*public void Draw(SpriteBatch spriteBatch)
    {
        icon.Draw(spriteBatch);
    }*/

    public bool CanUseItem(Player player)
    {
        if (quantity <= 0)
        {
            Console.WriteLine($"Not Enough {Type}");
            return false;
        }
        quantity--;
        Use(player);
        return true;
    }

}