using System.Collections.Generic;

namespace DrillDown;

public class Zone
{
    private int startingRow;
    private int endRow;
    private Dictionary<BlockType, float> spawnEntry = new();
    private BlockType defaultBlock;
    
    public BlockType DefaultBlock => defaultBlock;
    public Dictionary<BlockType, float> SpawnEntry => spawnEntry;
    public int StartingRow => startingRow;
    public int EndRow => endRow;

    public Zone(int startingRow, int endRow, Dictionary<BlockType, float> spawnEntry, BlockType defaultBlock)
    {
        this.startingRow = startingRow;
        this.endRow = endRow;
        this.spawnEntry = spawnEntry;
        this.defaultBlock = defaultBlock;
    }
}