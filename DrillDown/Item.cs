using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public enum ItemType{Bomb,Teleport,FuelTank,RepairKit}
public abstract class Item : IUsable
{
    private ItemType type;
    private Sprite icon;
    private int cost;
    private int quantity;
    private float cooldown;
    private float lastUseTime;
    public event Action<string,Vector2> OnUse;
    protected void OnItemUse(string effectName, Vector2 position) => OnUse?.Invoke(effectName, position);

    public ItemType Type => type;
    public int Cost => cost;
    public Sprite Icon => icon;
    public int Quantity => quantity;
    public float Cooldown => cooldown;
    public float LastUseTime => lastUseTime;
    public bool IsReady(float time) => time - lastUseTime >= cooldown;
    public void MarkUsed(float time) => lastUseTime = time;

    public Item(ItemType type, Sprite icon, int cost,float cooldown)
    {
        this.type = type;
        this.icon = icon;
        this.cost = cost;
        this.cooldown = cooldown;
    }
    
    public abstract void Use(Player player);
    public void AddQuantity(int amount) => quantity += amount;


    public bool TryUseItem(Player player)
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