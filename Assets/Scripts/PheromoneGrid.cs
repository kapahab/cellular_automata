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
    [Header("Debug Visuals")]
    public bool showDebugHeatmap = true;
    public float maxPheromoneValue = 100f; // Used to calculate the color intensity
    public Color debugColor = Color.blue;

    private Texture2D debugTexture;
    private Color[] debugPixels;
    private SpriteRenderer debugSpriteRenderer;

    public float[,] pheromoneGrid; //grid between 100.0f and 0 .0f, 1.0f being the strongest pheromone and 0.0f being no pheromone
    void Start()
    {
        PheromoneManipulation.currentPheromoneGrid = this;
    }

    // Update is called once per frame
    void Update()
    {
        DecayPheromone();
        if (showDebugHeatmap && pheromoneGrid != null)
        {
            DrawDebugOverlay();
        }
        else if (!showDebugHeatmap && debugSpriteRenderer != null)
        {
            // Instantly clear the overlay if we toggle it off
            debugSpriteRenderer.sprite = null;
        }
    }

    public void FillPheromoneGrid(int x, int y)
    {
        pheromoneGrid = new float[x, y];

        debugTexture = new Texture2D(x, y);
        debugTexture.filterMode = FilterMode.Point; 
        debugPixels = new Color[x * y];

        debugSpriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        transform.position = new Vector3(0, 0, -1f);
        debugSpriteRenderer.sprite = Sprite.Create(debugTexture, new Rect(0, 0, x, y), Vector2.zero, 1f);

    }

    void DecayPheromone()
    {
        if (pheromoneGrid == null)
            return;
        for (int i = 0; i < pheromoneGrid.GetLength(0); i++)
        {
            for (int j = 0; j < pheromoneGrid.GetLength(1); j++)
            {
                pheromoneGrid[i, j] *= 0.99f; // decay rate of 1% per frame, adjust as needed

                if (pheromoneGrid[i, j] < 0.01f)
                {
                    pheromoneGrid[i, j] = 0f;
                }
            }
        }
    }

    void DrawDebugOverlay()
    {
        int width = pheromoneGrid.GetLength(0);
        int height = pheromoneGrid.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                float scent = pheromoneGrid[x, y];

                if (scent > 0)
                {
                    // Normalize the scent value to a 0.0 to 1.0 range
                    float intensity = Mathf.Clamp01(scent / maxPheromoneValue);

                    // Apply the intensity directly to the Alpha channel of the blue color
                    debugPixels[index] = new Color(debugColor.r, debugColor.g, debugColor.b, intensity);
                }
                else
                {
                    // Completely transparent if there is no scent
                    debugPixels[index] = Color.clear;
                }
            }
        }

        // Push the visual overlay to the GPU
        debugTexture.SetPixels(debugPixels);
        debugTexture.Apply();
    }


}
