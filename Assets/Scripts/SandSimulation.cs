using UnityEngine;

public enum CellState
{
    Empty,
    Sand,
    HardenedSand,
    Food
}

public class SandSimulation : MonoBehaviour //PROJEDE YAPILACAKLAR, KARINCA SISTEMI GETIR
{
    [Header("Grid Settings")]
    public int width = 100; //statics can be removed later
    public int height = 100;
    public int circleRadius = 5;

    [Header("Visuals")]
    public Color sandColor = new Color(0.76f, 0.69f, 0.50f);
    public Color emptyColor = Color.black;

    // The pure data representation of our grid
    public CellState[,] grid;

    // The visual representation
    private Texture2D texture;
    private Color[] pixels;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private PheromoneGrid pheromoneGrid;

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
    void Start()
    {
        SandManipulation.currentSandEnvironment = this;
        // 1. Initialize the data grid
        grid = new CellState[width, height];
        pixels = new Color[width * height];
        pheromoneGrid.FillPheromoneGrid(width, height);

        // 2. Setup the Texture2D
        texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point; // Keeps the pixel art look crisp

        // 3. Setup the SpriteRenderer
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        // Create a sprite with a pivot at (0,0) and a Pixels Per Unit of 1. 
        // This makes 1 pixel exactly equal to 1 Unity World Unit, making input math trivial.
        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.zero, 1f);

        // Fill initially with empty color
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = emptyColor;
        }
        texture.SetPixels(pixels);
        texture.Apply();

        // Center the camera
        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);
        Camera.main.orthographicSize = height / 2f;
    }

    void Update()
    {
        HandleInput();
        Simulate();
        Draw();
    }

    void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Convert world position directly to grid coordinates
            int x = Mathf.FloorToInt(mouseWorldPos.x);
            int y = Mathf.FloorToInt(mouseWorldPos.y);

            // Bounds check
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                grid[x, y] = CellState.Sand;
            }
        }

        //if (Input.GetMouseButton(1))
        //{
        //    //bi yuvarlak halinde coklu kum atma
        //    Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //    int x = Mathf.FloorToInt(mouseWorldPos.x);
        //    int y = Mathf.FloorToInt(mouseWorldPos.y);

        //    for (int i = x - circleRadius;  i < x+ circleRadius; i++)
        //    {
        //        for(int j = y- circleRadius; j < y+ circleRadius; j++)
        //        {
        //            if ((i - x) * (i - x) + (j - y) * (j - y) > circleRadius * circleRadius) // daire icinde mi kontrolu
        //                continue;
        //            if (i >= 0 && i < width && j >= 0 && j < height)
        //            {
        //                grid[i, j] = CellState.Sand;
        //            }
        //        }
        //    }
        //}
    }



    void Simulate()
    {
        // Iterate from BOTTOM to TOP. 
        // If we iterate top-down, a sand particle would fall multiple times in a single frame.
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] == CellState.Sand || grid[x, y] == CellState.Food)
                {
                    FallingParticle(grid[x, y], x, y);
                }

                //else if (grid[x, y] == CellState.HardenedSand)
                //{
                //    bool isSupported = false;

                //    for (int i = 0; i < neighborDirections.Length; i++)
                //    {
                //        int checkX = x + neighborDirections[i].x;
                //        int checkY = y + neighborDirections[i].y;

                //        if (SandManipulation.CheckBounds(checkX, checkY))
                //        {
                //            CellState neighborCell = grid[checkX, checkY];

                //            if (neighborCell != CellState.Empty && neighborCell != CellState.HardenedSand)
                //            {

                //                isSupported = true;
                //                break;
                //            }
                //        }
                //    }

                //    if (!isSupported)
                //    {
                //        grid[x, y] = CellState.Empty;
                //    }
                //}
            }
        }
    }


    void FallingParticle(CellState fallingCell,int x, int y)
    {
        if (y > 0 && grid[x, y - 1] == CellState.Empty)
        {
            grid[x, y] = CellState.Empty;
            grid[x, y - 1] = fallingCell;
        }
        // Move Down-Left
        else if (y > 0 && x > 0 && grid[x - 1, y - 1] == CellState.Empty)
        {
            grid[x, y] = CellState.Empty;
            grid[x - 1, y - 1] = fallingCell;
        }
        // Move Down-Right
        else if (y > 0 && x < width - 1 && grid[x + 1, y - 1] == CellState.Empty)
        {
            grid[x, y] = CellState.Empty;
            grid[x + 1, y - 1] = fallingCell;
        }
    }

    void Draw()
    {
        // Update the 1D pixel array based on the 2D grid state
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Convert 2D coordinates to a 1D array index
                int index = y * width + x;
                if (grid[x, y] == CellState.Empty)
                {
                    pixels[index] = emptyColor;
                }
                else if (grid[x, y] == CellState.Sand)
                {
                    pixels[index] = sandColor;
                }
                else if (grid[x, y] == CellState.HardenedSand)
                {
                    pixels[index] = Color.gray;
                }
                else if (grid[x, y] == CellState.Food)
                {
                    pixels[index] = Color.green;
                }

            }
        }

        // Push the pixel data to the GPU. This is highly optimized.
        texture.SetPixels(pixels);
        texture.Apply();
    }

    
}