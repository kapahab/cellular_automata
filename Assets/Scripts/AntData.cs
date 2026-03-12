using UnityEngine;

public class AntData 
{
    public Vector2Int position;

    public Vector2Int nestPosition;

    public Vector2Int previousPosition;

    public AntBehaviour currentBehaviour;

    public float moveTimer = 0f;

    public Transform antTransform;

    public float currentPheromoneAmount;
}
