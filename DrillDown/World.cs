using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrillDown;

public class World : IDrawable
{
    private Block[,] world;
    private int blockSize;
    private Vector2 groundLevel;
    private int rows, columns;
    private float layer;
    public event Action<string,Vector2> OnBlockBreak;
    
    
    public World(Block[,] world, int blockSize, Vector2 groundLevel,float layer)
    {
        this.world = world;
        this.blockSize = blockSize;
        this.groundLevel = groundLevel;
        this.layer = layer;
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
                spriteBatch.Draw(block.Texture, dest,null ,Color.White,0f,Vector2.Zero,SpriteEffects.None,layer);
            }
        }
    }
    
    public bool IsSolid(Rectangle podRect)
    {
        int leftCol   = (int)Math.Floor((podRect.Left - groundLevel.X) / blockSize);
        int rightCol  = (int)Math.Floor((podRect.Right - 1 - groundLevel.X) / blockSize);
        int topRow    = (int)Math.Floor((podRect.Top - groundLevel.Y) / blockSize);
        int bottomRow = (int)Math.Floor((podRect.Bottom - 1 - groundLevel.Y) / blockSize);

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
        int row = (int)Math.Floor((position.Y - groundLevel.Y) / blockSize);
        return row;
    }

    public int WorldToCol(Vector2 position)
    {
        int col = (int)Math.Floor((position.X - groundLevel.X) / blockSize);
        return col;
    }
    

    public Material Drill(int row, int col, float deltaTime)
    {
        if (row < 0 || row >= rows || col < 0 || col >= columns) return null;

        Block block = world[row, col];
        if (block == null) return null;
        if (!block.IsBreakable) return null;
        bool broken = block.isDrilled(deltaTime);
        if (broken)
        {
            Vector2 effectPos = new Vector2(groundLevel.X + col * blockSize + blockSize * 0.5f,
                groundLevel.Y + row * blockSize + blockSize * 0.5f);
            OnBlockBreak?.Invoke(block.EffectName,effectPos);
            Material dropped = block.Material;
            world[row, col] = null;
            return dropped;
        }
        return null;
    }

    public void SetBlockUnbreakable(int row, int col)
    {
        if (row < 0 || row >= rows || col < 0 || col >= columns) return;
        Block block = world[row, col];
        if (block == null) return;
     
        block.SetBlockUnbreakable();
    }

    public void DestroyArea(Vector2 position, int radius)
    {
        int row = WorldToRow(position);
        int col = WorldToCol(position);
        for (int i = row - radius; i <= row + radius; i++)
        {
            for (int j = col - radius; j <= col + radius; j++)
            {
                if (i < 0 || i >= rows || j < 0 || j >= columns) continue;
                Block block = world[i, j];
                if (block == null || !block.IsBreakable) continue;
                Vector2 effectPos = new Vector2(groundLevel.X + j * blockSize + blockSize * 0.5f,
                    groundLevel.Y + i * blockSize + blockSize * 0.5f);
                OnBlockBreak?.Invoke(block.EffectName,effectPos);
                world[i, j] = null;
            }
        }
    }

    public float GetWorldRight() => groundLevel.X + columns * blockSize;
    public float GetWorldLeft() => groundLevel.X;
    public float GetWorldTop() => groundLevel.Y;
    public float GetWorldBottom() => groundLevel.Y + rows * blockSize;
    
}