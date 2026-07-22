using System.Collections.Generic;

namespace DrillDown;

public class WorldGenerator
{
    private int columns;
    private int rows;
    private List<BlockType> blockTypes;
    
    
    public WorldGenerator(int rows, int columns,List<BlockType> blockTypes)
    {
        this.columns = columns;
        this.rows = rows;
        this.blockTypes = blockTypes;
    }

    public Block[,] GenerateWorld()
    {
        Block[,] grid = new Block[rows, columns];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                BlockType type = r == 0 ? blockTypes[0] :
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
                grid[r, c] = new Block(type);
            }
        }
        return grid;
    }
}