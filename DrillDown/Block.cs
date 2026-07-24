using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class Block
{
    private BlockType type;
    private float drillProgress;
    private bool isBreakable;
    
    public bool IsBreakable => isBreakable;
    public Texture2D Texture => type.texture;
    public Material Material => type.material;

    

    public Block(BlockType type)
    {
        this.type = type;
        isBreakable = true;
    }
    
    public bool isDrilled(float amount)
    {
        drillProgress += amount;
        return drillProgress >= type.timeToDrill;
    }

    public void SetBlockUnbreakable()
    {
        isBreakable = false;
    }
}