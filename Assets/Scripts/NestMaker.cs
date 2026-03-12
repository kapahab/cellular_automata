using UnityEngine;
using UnityEngine.Rendering;

public class NestMaker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int circleRadius;
    public int fluctuationAmount;

    [SerializeField] private GameObject queenInstance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MakeNest();
            SpawnQueen(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    void MakeNest()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int x = Mathf.FloorToInt(mouseWorldPos.x);
        int y = Mathf.FloorToInt(mouseWorldPos.y);

        for (int i = x - circleRadius; i < x + circleRadius; i++)
        {
            for (int j = y - circleRadius; j < y + circleRadius; j++)
            {
                if ((i - x) * (i - x) + (j - y) * (j - y) > circleRadius * circleRadius) // daire icinde mi kontrolu
                    continue;

                if (SandManipulation.CheckBounds(i, j))
                    SandManipulation.GetGrid()[i, j] = CellState.Empty;

                
            }
        }

        int randomOffset = Random.Range(0, fluctuationAmount);

        for (int i = x - circleRadius; i < x + circleRadius; i++)
        {
            for (int j = y - circleRadius; j < y + circleRadius; j++)
            {
                if ((i - x) * (i - x) + (j - y) * (j - y) > (circleRadius) * (circleRadius)) // randomize etmek istiyorum
                    continue;
                SandManipulation.HardenSand(i, j);
            }

        }
    }

    void SpawnQueen(Vector3 point)
    {
        Instantiate(queenInstance, new Vector3(point.x,point.y,0), Quaternion.identity);
    }

}
