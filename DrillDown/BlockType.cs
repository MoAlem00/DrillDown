using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class BlockType
{
    public readonly string name;
    public readonly Texture2D? texture;
    public readonly float timeToDrill;
    public readonly Material? material;
    public readonly string effectName;
    
    public BlockType(string name, Texture2D texture, float timeToDrill,Material material = null, string effectName = "BreakEffect")
    {
        this.name = name;
        this.texture = texture;
        this.timeToDrill = timeToDrill;
        this.material = material;
        this.effectName = effectName;
    }
}