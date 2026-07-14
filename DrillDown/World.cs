using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class World : IDrawable
{
    private Block[,] world;
    private int blockSize;
    private Vector2 groundLevel;
    private int rows, columns;
    
    
    public World(Block[,] world, int blockSize, Vector2 groundLevel)
    {
        this.world = world;
        this.blockSize = blockSize;
        this.groundLevel = groundLevel;
        rows = world.GetLength(0);
        columns = world.GetLength(1);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        for (int r = 0; r < rows ; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Block block = world[r, c];
                Rectangle dest = new Rectangle(
                    (int)groundLevel.X + c * blockSize,
                    (int)groundLevel.Y + r * blockSize,
                    blockSize,
                    blockSize);
                spriteBatch.Draw(block.Texture, dest, Color.White);
            }
        }
    }
}