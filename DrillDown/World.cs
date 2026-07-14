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
    
    public bool IsSolid(Rectangle podRect)
    {
        int leftCol   = (int)((podRect.Left   - groundLevel.X) / blockSize);
        int rightCol  = (int)((podRect.Right - 1  - groundLevel.X) / blockSize);
        int topRow    = (int)((podRect.Top    - groundLevel.Y) / blockSize);
        int bottomRow = (int)((podRect.Bottom - 1 - groundLevel.Y) / blockSize);

        for (int r = topRow; r <= bottomRow; r++)
        {
            for (int c = leftCol; c <= rightCol; c++)
            {
                if (r < 0 || r >= rows || c < 0 || c >= columns) continue;
                if (world[r, c] != null) return true;
            }
        }
        return false;
    }
}