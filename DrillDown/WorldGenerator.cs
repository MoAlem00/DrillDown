using System;
using System.Collections.Generic;

namespace DrillDown;

public class WorldGenerator
{
    private int columns;
    private int rows;
    private List<Zone> zones;
    private Random random = new Random();
    
    public WorldGenerator(int rows, int columns, List<Zone> zones)
    {
        this.columns = columns;
        this.rows = rows;
        this.zones = zones;
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
                if (currentZone == null)
                    Console.WriteLine($"no zone for row {r}");
            }
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

                if (random.NextDouble() < 0.02)
                    grid[r, c] = null;
                else
                    grid[r, c] = new Block(chosenBlock);
            }
        }
        return grid;
    }
}

/*BlockType type = r == 0 ? blockTypes[0] :
                    r < 3 ? blockTypes[1] :
                    r < 4 ? blockTypes[2] :
                    r < 5 ? blockTypes[3] :
                    r < 6 ? blockTypes[4] :
                    r < 7 ? blockTypes[5] :
                    r < 8 ? blockTypes[6] :
                    r < 9 ? blockTypes[7] :
                    r < 10 ? blockTypes[8] :
                    r < 11 ? blockTypes[9] :
                    blockTypes[10];
                grid[r, c] = new Block(type);*/