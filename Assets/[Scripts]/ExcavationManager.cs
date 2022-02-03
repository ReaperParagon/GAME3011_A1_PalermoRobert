using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExcavationManager : MonoBehaviour
{
    [Header("Grid Information")]
    [SerializeField]
    private GameObject GridArea;
    private GridLayoutGroup gridLayout;

    [SerializeField]
    private Vector2Int GridDimensions;
    private int GridCount { get { return GridDimensions.x * GridDimensions.y; } }

    private List<List<ResourceTile>> GridTiles = new List<List<ResourceTile>>();

    [Header("Grid Tile Information")]
    [SerializeField]
    private GameObject GridTilePrefab;
    [SerializeField]
    private int extractionAttempts = 3;
    public int extractionsLeft;
    [SerializeField]
    private int scanAttempts = 6;
    public int scansLeft;

    [Header("Resource Information")]
    [SerializeField]
    private int numMaxResourceTiles = 40;
    [SerializeField]
    private int maxResourceValue = 2000;
    public int extractedValue = 0;

    public bool IsScanning = true;

    public UnityEvent<int> ExtractedValueUpdated = new UnityEvent<int>();
    public UnityEvent<int> FinishedExcavation = new UnityEvent<int>();
    public UnityEvent<int> ScanUsed = new UnityEvent<int>();
    public UnityEvent<int> ExtractUsed = new UnityEvent<int>();
    public UnityEvent<bool> ChangeMode = new UnityEvent<bool>();

    public void ResetValues()
    {
        extractedValue = 0;
        ExtractedValueUpdated.Invoke(0);

        extractionsLeft = extractionAttempts;
        ExtractUsed.Invoke(extractionsLeft);

        scansLeft = scanAttempts;
        ScanUsed.Invoke(scansLeft);

        IsScanning = true;
        ChangeMode.Invoke(IsScanning);

        // Reset Values of each tile, set new values
        foreach (List<ResourceTile> row in GridTiles)
        {
            foreach (ResourceTile rTile in row)
            {
                rTile.ResetValues();
            }
        }

        PlaceResources();
    }

    private void Awake()
    {
        // Setup Grid Layout
        gridLayout = GridArea.GetComponent<GridLayoutGroup>();
        gridLayout.constraintCount = GridDimensions.x;

        for (int i = 0; i < GridCount; i++)
        {
            // Add Tile to Grid
            GameObject tile = Instantiate(GridTilePrefab, GridArea.transform);

            // Store info based on all grid positions
            int gridY = i / GridDimensions.x;
            int gridX = i % GridDimensions.x;

            ResourceTile rTile = tile.GetComponent<ResourceTile>();

            rTile.SetTile(tile);
            rTile.SetGridPosition(gridX, gridY);

            // Check our Grid size
            if (gridX > GridTiles.Count - 1)
                GridTiles.Add(new List<ResourceTile>());

            if (gridY > GridTiles[gridX].Count - 1)
                GridTiles[gridX].Add(rTile);

            GridTiles[gridX][gridY] = rTile;
        }

        foreach (List<ResourceTile> row in GridTiles)
        {
            foreach (ResourceTile rTile in row)
            {
                // Set up neighbouring tiles
                rTile.CloseTiles.AddRange(GetCloseTiles(rTile.GridPosition));
            }
        }

        ResetValues();
    }

    private List<ResourceTile> GetCloseTiles(Vector2Int gridPosition)
    {
        List<ResourceTile> rTileList = new List<ResourceTile>();

        int x = gridPosition.x;
        int y = gridPosition.y;

        int gridRight = GridTiles.Count - 1;

        // Left
        if (x - 1 >= 0)
        {
            rTileList.Add(GetResourceTileAtPosition(x - 1, y));

            // Top Left
            if (y > 0)
            {
                rTileList.Add(GetResourceTileAtPosition(x - 1, y - 1));
            }

            // Bottom Left
            if (y < GridTiles[0].Count - 1)
            {
                rTileList.Add(GetResourceTileAtPosition(x - 1, y + 1));
            }
        }

        // Right
        if (x + 1 <= gridRight)
        {
            rTileList.Add(GetResourceTileAtPosition(x + 1, y));

            // Top Right
            if (y > 0)
            {
                rTileList.Add(GetResourceTileAtPosition(x + 1, y - 1));
            }

            // Bottom Right
            if (y < GridTiles[0].Count - 1)
            {
                rTileList.Add(GetResourceTileAtPosition(x + 1, y + 1));
            }
        }

        // Top
        if (y > 0)
        {
            rTileList.Add(GetResourceTileAtPosition(x, y - 1));
        }

        // Bottom
        if (y < GridTiles[0].Count - 1)
        {
            rTileList.Add(GetResourceTileAtPosition(x, y + 1));
        }

        return rTileList;
    }

    private ResourceTile GetResourceTileAtPosition(int x, int y)
    {
        return GridTiles[x][y];
    }

    public void PlaceResources()
    {
        for (int i = 0; i < numMaxResourceTiles; i++)
        {
            int randX = Random.Range(0, GridDimensions.x);
            int randY = Random.Range(0, GridDimensions.y);

            // Place resources in pattern from this tile
            ResourceTile rTile = GridTiles[randX][randY];

            // Check if that square is a max resource, if it is then try another tile
            if (rTile.TileValue == ResourceValue.Max)
            {
                i--;
                continue;
            }

            rTile.SetResourceValue(ResourceValue.Max);
            rTile.SetSurroundingTileResourceValues(ResourceValue.Half, false);
        }
    }

    public void Extract(ResourceValue value)
    {
        int valueGained;

        switch (value)
        {
            case ResourceValue.Max:
                valueGained = maxResourceValue;
                break;
            case ResourceValue.Half:
                valueGained = (int)(maxResourceValue * 0.5f);
                break;
            case ResourceValue.Quarter:
                valueGained = (int)(maxResourceValue * 0.25f);
                break;
            case ResourceValue.Min:
                valueGained = (int)(maxResourceValue * 0.125f);
                break;
            default:
                valueGained = (int)(maxResourceValue * 0.125f);
                break;
        }

        extractedValue += valueGained;
        extractionsLeft--;

        ExtractedValueUpdated.Invoke(valueGained);
        ExtractUsed.Invoke(extractionsLeft);

        // Reduce Extractions, Check if out of extractions
        if (extractionsLeft <= 0)
        {
            // Out of extractions, go to results
            FinishedExcavation.Invoke(extractedValue);
        }
    }

    public void ResetVisitedTiles()
    {
        foreach (List<ResourceTile> row in GridTiles)
        {
            foreach (ResourceTile rTile in row)
            {
                rTile.Visited = false;
            }
        }
    }
}
