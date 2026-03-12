using UnityEngine;

public static class PheromoneManipulation
{
    // Everything inside must be static
    public static PheromoneGrid currentPheromoneGrid;

    public static float CheckPheromoneGrid(int x, int y)
    {
        return currentPheromoneGrid.pheromoneGrid[x,y];
    }

    public static void AddPheromoneToGrid(int x, int y, float amount)
    {
        currentPheromoneGrid.pheromoneGrid[x, y] = Mathf.Max(currentPheromoneGrid.pheromoneGrid[x, y], amount);
        if (currentPheromoneGrid.pheromoneGrid[x, y] > 100.0f)
        {
            currentPheromoneGrid.pheromoneGrid[x, y] = 100.0f;
        }
        else if (currentPheromoneGrid.pheromoneGrid[x, y] < 0.0f)
        {
            currentPheromoneGrid.pheromoneGrid[x, y] = 0.0f;
        }
    }

}
