using UnityEngine;

public class AntSensors : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public CellState LookAt(int x, int y)
    {
        if (SandManipulation.CheckBounds(x, y))
        {
            return SandManipulation.GetGrid()[x, y];
        }

        return CellState.Empty;
    }
}
