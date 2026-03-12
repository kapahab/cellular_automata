using UnityEngine;

public class AntPhysics : MonoBehaviour //for now only needs to check if there is sand underneath it and if not it needs to move down.
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //CheckForGround();
    }

    void CheckForGround()
    {
        float sandX = Mathf.Floor(transform.position.x);
        float sandYminOne = Mathf.Floor(transform.position.y - 1f); // Check the cell directly below the ant

        if (sandX >= 0 && sandX < SandManipulation.GetWidth() && sandYminOne >= 0 && sandYminOne < SandManipulation.GetHeight())
        {
            if (SandManipulation.GetGrid()[(int)sandX, (int)sandYminOne] == CellState.Sand)
            {
                // There is sand below, do nothing
                return;
            }
            transform.position = new Vector3(sandX, sandYminOne, 0); // Ensure the ant's position is aligned to the grid
        }
        else
        {
            Debug.LogWarning("Ant outside simulation space!");
        }
    }
}
