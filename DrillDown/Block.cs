using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class Block
{
    private BlockType type;
    private float drillProgress;
    public Collider collider;
    public Rectangle bounds;
    
    public Texture2D Texture => type.texture;
    public Material Material => type.material;

    

    public Block(BlockType type)
    {
        this.type = type;
    }
    
}