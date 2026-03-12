using UnityEngine;
using System.Collections.Generic;

public class AntManager : MonoBehaviour
{
    public List<AntData> allAnts = new List<AntData>();

    public float globalMoveSpeed = 0.2f;

    [SerializeField] private GameObject antPrefab;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (AntData ant in allAnts)
        {
            ant.moveTimer += Time.deltaTime;

            if (ant.moveTimer >= globalMoveSpeed)
            {
                ant.moveTimer = 0f; 

                ProcessAntMovement(ant);
            }
        }
    }

    void ProcessAntMovement(AntData ant)
    {
        //first condition: if ant is on food, pick it up and change behaviour to carrying food
        //if ant is carrying food, get it to the nest
        int targetX;
        int targetY;
        if (ant.currentBehaviour == AntBehaviour.CarryingFood)
        {
            if (ant.position == ant.nestPosition)
            {
                DropFoodAtNest(ant);
                return;
            }
            else
            {
                int diffX = ant.nestPosition.x - ant.position.x;
                int diffY = ant.nestPosition.y - ant.position.y;

                int stepX = System.Math.Sign(diffX);
                int stepY = System.Math.Sign(diffY);

                targetX = ant.position.x + stepX;
                targetY = ant.position.y + stepY;

                AdjustAntPheromone(ant);
                MoveAnt(ant, targetX, targetY);
                return;
            }
        }


        float highestPheromone = 0f;
        Vector2Int bestDirection = Vector2Int.zero;
        for (int i = 0; i < neighborDirections.Length; i++)
        {
            Vector2Int dir = neighborDirections[i];
            int checkX = ant.position.x + dir.x;
            int checkY = ant.position.y + dir.y;
            if (!SandManipulation.CheckBounds(checkX, checkY))
            {
                continue;
            }
            if (checkX == ant.previousPosition.x && checkY == ant.previousPosition.y)
            {
                continue;
            }
            CellState cellState = SandManipulation.GetGrid()[checkX, checkY];
            if (cellState == CellState.Food)
            {
                MoveAnt(ant, checkX, checkY);
                SandManipulation.GetGrid()[checkX, checkY] = CellState.Empty;
                ChangeAntState(ant, AntBehaviour.CarryingFood);
                return;
            }
            else
            {

                float foodPheromoneLevel = PheromoneManipulation.CheckPheromoneGrid(checkX, checkY);
                if (foodPheromoneLevel > highestPheromone)
                {
                    highestPheromone = foodPheromoneLevel;
                    bestDirection = dir;
                }

            }
        }

        if (highestPheromone > 0f)
        {
            targetX = ant.position.x + bestDirection.x;
            targetY = ant.position.y + bestDirection.y;
            MoveAnt(ant, targetX, targetY);
            return;
        }

        Vector2Int chosenDir = neighborDirections[Random.Range(0, neighborDirections.Length)];
        targetX = ant.position.x + chosenDir.x;
        targetY = ant.position.y + chosenDir.y;

        MoveAnt(ant, targetX, targetY);

        //if (!SandManipulation.CheckBounds(targetX, targetY)) return;

        //CellState nextCell = SandManipulation.GetGrid()[targetX, targetY];

        //if (nextCell == CellState.Empty)
        //{
        //    MoveAnt(ant, targetX, targetY);
        //}
        //else if (nextCell == CellState.Sand || nextCell == CellState.HardenedSand)
        //{
        //    SandManipulation.Dig(targetX, targetY);
        //    MoveAnt(ant, targetX, targetY);
        //}
    }

    void MoveAnt(AntData ant, int targetX, int targetY)
    {
        ant.previousPosition = ant.position;
        if (!SandManipulation.CheckBounds(targetX, targetY)) return;

        CellState nextCell = SandManipulation.GetGrid()[targetX, targetY];

        if(nextCell == CellState.Sand || nextCell == CellState.HardenedSand)
        {
            SandManipulation.Dig(targetX, targetY);
        }

        ant.position = new Vector2Int(targetX, targetY);

        if (ant.antTransform != null)
        {
            ant.antTransform.position = new Vector3(targetX + 0.5f, targetY + 0.5f, 0);
        }
    }

    void ChangeAntState(AntData ant, AntBehaviour newState)
    {
        ant.currentBehaviour = newState;
    }

    void DropFoodAtNest(AntData ant)
    {
        ant.currentBehaviour = AntBehaviour.Random;
        ant.currentPheromoneAmount = 0f;
    }

    void AdjustAntPheromone(AntData ant) //make ant do a blast of pheromone around food so the ants are incetivized to roam around food
    {
        if (ant.currentBehaviour == AntBehaviour.CarryingFood)
        {
            if (ant.currentPheromoneAmount == 0f)
            {
                ant.currentPheromoneAmount = 100f; // Example initial pheromone amount
            }
            else
            {
                ant.currentPheromoneAmount = ant.currentPheromoneAmount - 0.5f; // Example decay rate
            }
            // Increase food pheromone
            PheromoneManipulation.AddPheromoneToGrid(ant.position.x, ant.position.y, ant.currentPheromoneAmount); // Example value
        }

    }


    public void InstantiateAnt(Vector2Int currentPos, Vector2Int homePos)
    {
        GameObject antInstance = Instantiate(antPrefab, new Vector3(currentPos.x + 0.5f, currentPos.y + 0.5f, 0), Quaternion.identity);
        AntData newAnt = new AntData();
        newAnt.position = currentPos; // Example starting position
        newAnt.nestPosition = homePos; // Example nest position
        newAnt.antTransform = antInstance.transform;
        allAnts.Add(newAnt);
    }
}
