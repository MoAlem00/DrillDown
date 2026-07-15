using System;
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
                if(block == null) continue;
                Rectangle dest = new Rectangle(
                    (int)groundLevel.X + c * blockSize,
                    (int)groundLevel.Y + r * blockSize,
                    blockSize,
                    blockSize);
                spriteBatch.Draw(block.Texture, dest, Color.White);
            }
        }
    }
    
    public Rectangle CellRect(int r, int c) => new Rectangle(
        (int)groundLevel.X + c * blockSize,
        (int)groundLevel.Y + r * blockSize,
        blockSize, blockSize);
    
    public bool IsSolid(Rectangle podRect)
    {
        int leftCol   = (int)Math.Floor((podRect.Left       - groundLevel.X) / (float)blockSize);
        int rightCol  = (int)Math.Floor((podRect.Right  - 1 - groundLevel.X) / (float)blockSize);
        int topRow    = (int)Math.Floor((podRect.Top        - groundLevel.Y) / (float)blockSize);
        int bottomRow = (int)Math.Floor((podRect.Bottom - 1 - groundLevel.Y) / (float)blockSize);

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
    
    public int WorldToRow(Vector2 position)
    {
        int row = (int)Math.Floor((position.Y - groundLevel.Y) / (float)blockSize);
        return row;
    }

    public int WorldToCol(Vector2 position)
    {
        int col = (int)Math.Floor((position.X - groundLevel.X) / (float)blockSize);
        return col;
    }

    public void Drill(int row, int col, float deltaTime)
    {
        if (row < 0 || row >= rows || col < 0 || col >= columns) return;

        Block block = world[row, col];
        if (block == null) return;
        
        bool broken = block.isDrilled(deltaTime);
        if (broken)
            world[row, col] = null;
    }
}