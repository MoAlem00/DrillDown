using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DrillDown;

public class WorldGenerator
{
    private int columns;
    private int rows;
    private List<Zone> zones;
    private Random random = new Random();
    private BlockType bedrock;
    
    public WorldGenerator(int rows, int columns, List<Zone> zones)
    {
        this.columns = columns;
        this.rows = rows;
        this.zones = zones;
        bedrock = new BlockType("Bedrock", SpriteManager.GetSprite("BedrockBlock").texture, 0);
    }

    public Block[,] GenerateWorld()
    {
        Block[,] grid = new Block[rows, columns];
        BlockType chosenBlock;
        Zone currentZone = null;
        for (int r = 0; r < rows; r++)
        {
            foreach (var z in zones)
            {
                if (r >= z.StartingRow && r <= z.EndRow)
                {
                    currentZone = z;
                    break;
                }
            }
            if (currentZone == null)
                Console.WriteLine($"no zone for row {r}");
            for (int c = 0; c < columns; c++)
            {
                chosenBlock = currentZone.DefaultBlock;
                foreach (var entry in currentZone.SpawnEntry)
                {
                    if (random.NextDouble() < entry.Value)
                    {
                        chosenBlock = entry.Key;
                        break;
                    }
                }
                bool canBeHole = r > 2 && r < Game1.rows - 1;
                if (r == Game1.rows - 1)
                {
                    var lastBlock = new Block(bedrock);
                    lastBlock.SetBlockUnbreakable();
                    grid[r, c] = lastBlock;
                }
                else if (canBeHole && random.NextDouble() < 0.05)
                    grid[r, c] = null;
                else grid[r, c] = new Block(chosenBlock);
            }
        }
        return grid;
    }

}
