using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.FloorToInt(mouseWorldPos.x);
            int y = Mathf.FloorToInt(mouseWorldPos.y);

            SandManipulation.MakeFood(x, y);
        }
    }
}
