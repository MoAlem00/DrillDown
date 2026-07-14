using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class Material
{
    private readonly string name;
    private readonly Texture2D texture;
    private readonly float weight;
    private readonly int sellCost;
    
    public float Weight => weight;
    public string Name => name;
    public int SellCost => sellCost;

    public Material(string name, Texture2D texture, float weight, int sellCost)
    {
        this.name = name;
        this.texture = texture;
        this.weight = weight;
        this.sellCost = sellCost;
    }
    
}