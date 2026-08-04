using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class BlockType
{
    public readonly string name;
    public readonly Texture2D? texture;
    public readonly float timeToDrill;
    public readonly Material? material;
    
    public BlockType(string name, Texture2D texture, float timeToDrill,Material material = null)
    {
        this.name = name;
        this.texture = texture;
        this.timeToDrill = timeToDrill;
        this.material = material;
    }
}