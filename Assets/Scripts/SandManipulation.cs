using UnityEngine;

public static class SandManipulation
{
    // Everything inside must be static
    public static SandSimulation currentSandEnvironment;


    private static readonly Vector2Int[] neighborDirections = new Vector2Int[]
    {
        new Vector2Int(0, 1), // Up
        new Vector2Int(0, -1), // Down
        new Vector2Int(-1, 0), // Left
        new Vector2Int(1, 0), // Right
        new Vector2Int(-1, 1), // Up-Left
        new Vector2Int(1, 1), // Up-Right
        new Vector2Int(-1, -1), // Down-Left
        new Vector2Int(1, -1) // Down-Right
    };
    public static void HardenSand(int x, int y)
    {
        if (currentSandEnvironment == null) return;

        foreach (Vector2Int dir in neighborDirections)
        {
            int neighborX = x + dir.x;
            int neighborY = y + dir.y;

            if (CheckBounds(neighborX, neighborY))
            {
                if (currentSandEnvironment.grid[neighborX, neighborY] == CellState.Sand)
                {
                    currentSandEnvironment.grid[neighborX, neighborY] = CellState.HardenedSand;
                }
            }
        }
    }

    public static bool CheckBounds(int x, int y)
    {
        if (currentSandEnvironment == null) return false;
        return x >= 0 && x < currentSandEnvironment.width && y >= 0 && y < currentSandEnvironment.height;
    }

    public static int GetWidth() => currentSandEnvironment != null ? currentSandEnvironment.width : 0;
    public static int GetHeight() => currentSandEnvironment != null ? currentSandEnvironment.height : 0;
    public static CellState[,] GetGrid() => currentSandEnvironment?.grid;

    public static void Dig(int x, int y) //geldigi yeri bilmeli ki arkasini kapamasin 
    {
        if (!CheckBounds(x, y))
        {
            return;
        }

        if (GetGrid()[x, y] == CellState.Empty)
        {
            return;
        }

        GetGrid()[x, y] = CellState.Empty;
        HardenSand(x, y);
    }

}
