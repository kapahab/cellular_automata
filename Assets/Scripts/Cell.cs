using UnityEngine;


public class Cell : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    BoxCollider2D boxCollider2D;
    public CellState cellState = CellState.Empty;
    public SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsPressedThisFrame()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (boxCollider2D == null)
            {
                boxCollider2D = GetComponent<BoxCollider2D>();
            }
            bool check = boxCollider2D.OverlapPoint(mousePos); 
            return check;
        }
        return false;
    }
}