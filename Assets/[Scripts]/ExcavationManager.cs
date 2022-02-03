using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    // private ResourceTile[][] GridTiles;

    [Header("Grid Tile Information")]
    [SerializeField]
    private GameObject GridTilePrefab;
    [SerializeField]
    private int extractionAttempts = 3;
    private int extractionsLeft;
    [SerializeField]
    private int scanAttempts = 6;
    private int scansLeft;

    [Header("Resource Information")]
    [SerializeField]
    private int numMaxResourceTiles = 40;
    [SerializeField]
    private int maxResourceValue = 2000;
    public int extractedValue = 0;

    public bool IsScanning = true;

    private void ResetValues()
    {
        extractedValue = 0;
        extractionsLeft = extractionAttempts;
        scansLeft = scanAttempts;

        // Reset Values of each tile, set new values
        foreach (List<ResourceTile> row in GridTiles)
        {
            row.Clear();
        }
    }

    private void Awake()
    {
        ResetValues();

        // Setup Grid Layout
        gridLayout = GridArea.GetComponent<GridLayoutGroup>();
        gridLayout.constraintCount = GridDimensions.x;

        for (int i = 0; i < GridCount; i++)
        {
            // Add Tile to Grid
            GameObject tile = Instantiate(GridTilePrefab, GridArea.transform);

            // Store info based on all grid positions
            int gridX = i / GridDimensions.x;
            int gridY = i % GridDimensions.x;

            Debug.Log(gridX + "   " + gridY);

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
    }

    public void PlaceResources()
    {
        for (int i = 0; i < numMaxResourceTiles; i++)
        {
            int randX = Random.Range(0, GridDimensions.x);
            int randY = Random.Range(0, GridDimensions.y);

            // Place resources in pattern from this tile
            ResourceTile rTile = GridTiles[randX][randY];

            rTile.SetResourceValue(ResourceValue.Max);
            rTile.SetSurroundingTileResourceValues(ResourceValue.Half, false);
        }
    }

    public void Extract(ResourceValue value)
    {
        switch (value)
        {
            case ResourceValue.Max:
                extractedValue += maxResourceValue;
                break;
            case ResourceValue.Half:
                extractedValue += (int)(maxResourceValue * 0.5f);
                break;
            case ResourceValue.Quarter:
                extractedValue += (int)(maxResourceValue * 0.25f);
                break;
            case ResourceValue.Min:
                extractedValue += (int)(maxResourceValue * 0.125f);
                break;
            default:
                extractedValue += (int)(maxResourceValue * 0.125f);
                break;
        }

        // Reduce Extractions, Check if out of extractions
        if (--extractionsLeft <= 0)
        {
            // Out of extractions, go to results
        }
    }
}
