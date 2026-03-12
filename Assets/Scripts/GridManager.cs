using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int gridSizeX;
    public int gridSizeY;
    public float scaleFactor;
    [SerializeField] private GameObject cellPrefab;
    List<Cell> cells = new List<Cell>();
    public Sprite sandSprite, emptySprite;

    private Dictionary<Vector2Int, Cell> cellDictionary = new Dictionary<Vector2Int, Cell>();
    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        Camera.main.orthographicSize = Mathf.Max(gridSizeX, gridSizeY) / 2f;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 cellPosition = new Vector3(x, y, 0);
                GameObject cellInst = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);
                cellInst.name = $"Cell_{x}_{y}";
                cells.Add(cellInst.GetComponent<Cell>());
                cellDictionary.Add(new Vector2Int(x, y), cellInst.GetComponent<Cell>());
            }
        }

        float centerX = (gridSizeX - 1) / 2f;
        float centerY = (gridSizeY - 1) / 2f;

        Camera.main.transform.position = new Vector3(centerX, centerY, -10);
    }

    // Update is called once per frame
    void Update()
    {
        CheckCells();
    }


    void CheckCells()
    {
        foreach (Cell cell in cells)
        {
            if (cell.IsPressedThisFrame())
            {
                MakeSandTile(cell);
                Debug.Log("Cell Pressed: " + cell.name);
            }
            if (cell.cellState == CellState.Sand)
            {
                MoveSandDown(cell);

            }
        }
    }

    void MakeSandTile(Cell cell)
    {
        cell.cellState = CellState.Sand;
        cell.sr.sprite = sandSprite;
    }

    void MakeEmptyTile(Cell cell)
    {
        cell.cellState = CellState.Empty;
        cell.sr.sprite = emptySprite;
    }

    void MoveSandDown(Cell cell)
    {
        // Check if the cell below is empty
        Vector3 belowPosition = cell.transform.position + Vector3.down;
        Cell belowCell = cellDictionary.TryGetValue(new Vector2Int((int)belowPosition.x, (int)belowPosition.y), out Cell foundCell) ? foundCell : null;
        if (belowCell == null)
            return;
        if (belowCell.cellState == CellState.Empty)
        {
            MakeEmptyTile(cell);
            MakeSandTile(belowCell);
        }
        else if (belowCell.cellState == CellState.Sand)
        {
            // Check diagonally down-left
            Vector3 downLeftPosition = cell.transform.position + Vector3.down + Vector3.left;
            Cell downLeftCell = cellDictionary.TryGetValue(new Vector2Int((int)downLeftPosition.x, (int)downLeftPosition.y), out Cell foundDownLeftCell) ? foundDownLeftCell : null;
            if (downLeftCell != null && downLeftCell.cellState == CellState.Empty)
            {
                MakeEmptyTile(cell);
                MakeSandTile(downLeftCell);
            }
            else
            {
                // Check diagonally down-right
                Vector3 downRightPosition = cell.transform.position + Vector3.down + Vector3.right;
                Cell downRightCell = cellDictionary.TryGetValue(new Vector2Int((int)downRightPosition.x, (int)downRightPosition.y), out Cell foundDownRightCell) ? foundDownRightCell : null;
                if (downRightCell != null && downRightCell.cellState == CellState.Empty)
                {
                    MakeEmptyTile(cell);
                    MakeSandTile(downRightCell);
                }
            }
        }
    }
}
