using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class Block
{
    private BlockType type;
    private float drillProgress;
    private bool isBreakable;
    private Animation breakEffect;
    
    public bool IsBreakable => isBreakable;
    public Texture2D Texture => type.texture;
    public Material Material => type.material;
    public string EffectName => type.effectName;

    

    public Block(BlockType type)
    {
        this.type = type;
        breakEffect = new Animation(EffectName);
        isBreakable = true;
    }
    
    public bool IsDrilled(float amount)
    {
        drillProgress += amount;
        return drillProgress >= type.timeToDrill;
    }

    public void SetBlockUnbreakable()
    {
        isBreakable = false;
    }

}