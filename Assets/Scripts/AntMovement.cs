using System.Collections;
using Unity.Collections;
using UnityEngine;

public enum AntBehaviour
{
    Random,
    CarryingFood,
    FollowingFoodPheromone,
}
public class AntMovement : MonoBehaviour
{
    private static readonly Vector2Int[] neighborDirections = new Vector2Int[]
    {
        new Vector2Int(0, 1),   // Up
        new Vector2Int(0, -1),  // Down
        new Vector2Int(-1, 0),  // Left
        new Vector2Int(1, 0),   // Right
        new Vector2Int(-1, 1),  // Up-Left
        new Vector2Int(1, 1),   // Up-Right
        new Vector2Int(-1, -1), // Down-Left
        new Vector2Int(1, -1)   // Down-Right
    };

    [SerializeField] private float moveSpeed;

    private AntSensors antSensors;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        antSensors = GetComponent<AntSensors>();
        StartCoroutine(MoveRandomly());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void MoveAnt(int x, int y)
    {
        transform.position = new Vector3(x+0.5f, y+0.5f, 0);
    }
    IEnumerator MoveRandomly()
    {
        while (true)
        {
            int x = (int)Mathf.Floor(transform.position.x);
            int y = (int)Mathf.Floor(transform.position.y);

            Vector2Int chosenDir = neighborDirections[Random.Range(0, neighborDirections.Length)];

            MoveChecks(x + chosenDir.x, y + chosenDir.y);

            yield return new WaitForSeconds(moveSpeed);
        }

    }

    void MoveChecks(int x, int y)
    {
        if (!SandManipulation.CheckBounds(x, y))
        {
            return;
        }
        CellState nextCell = antSensors.LookAt(x, y );

        if (nextCell == CellState.Empty)
        {
            MoveAnt(x, y);
        }
        else if (nextCell == CellState.Sand || nextCell == CellState.HardenedSand)
        {
            SandManipulation.Dig(x, y );
            MoveAnt(x, y);
        }
        else
        {
            return;
        }
    }

}
