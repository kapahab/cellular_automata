using UnityEngine;

public enum PheromoneTypes
{
    Empty,
    Food,
    Nest,
    Bad
}
public class PheromoneGrid : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private PheromoneTypes[,] pheromoneGrid;
    void Start()
    {
        PheromoneManipulation.currentPheromoneGrid = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillPheromoneGrid(int x, int y)
    {
        pheromoneGrid = new PheromoneTypes[x, y];

    }
}
