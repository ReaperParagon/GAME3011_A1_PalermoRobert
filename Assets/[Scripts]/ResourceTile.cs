using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceValue
{
    Max,
    Half,
    Quarter,
    Min,
    Extracted
}

public class ResourceTile : MonoBehaviour
{
    [Header("TileInformation")]
    public GameObject Tile;
    public ResourceValue TileValue = ResourceValue.Min;
    public Vector2Int GridPosition = new Vector2Int(0, 0);

    public List<ResourceTile> CloseTiles = new List<ResourceTile>();

    public bool IsScanned = false;

    public static ExcavationManager excavationManager;

    public ExcavationManager ExcavationManager
    {
        get
        {
            if (excavationManager == null)
                excavationManager = FindObjectOfType<ExcavationManager>();

            return excavationManager;
        }
    }

    public void ResetValues()
    {
        CloseTiles.Clear();
        TileValue = ResourceValue.Min;
        IsScanned = false;
    }

    private void Awake()
    {
        if (excavationManager == null)
            return;

        excavationManager = FindObjectOfType<ExcavationManager>();
    }

    public void SetTile(GameObject tile)
    {
        Tile = tile;
    }

    public void SetGridPosition(int x, int y)
    {
        GridPosition = new Vector2Int(x, y);
    }

    public void SetResourceValue(ResourceValue value)
    {
        TileValue = value;
    }

    public void OnClick()
    {
        bool isScanning = ExcavationManager.IsScanning;

        if (isScanning)
            OnScan();
        else
            OnExtract();
    }

    private void OnScan()
    {
        // Reveal this tile and the surrounding 8 tiles
    }

    private void OnExtract()
    {
        if (TileValue == ResourceValue.Extracted)
        {
            // Print information that you cannot extract that tile

            return;
        }

        // Extract this tile's resources
        // Reduce resources of surrounding 24 tiles
    }

    public void UpdateTile()
    {
        // Set Tile's Colour based on Tile Value
        Color tileColour;

        switch (TileValue)
        {
            case ResourceValue.Max:
                tileColour = Color.yellow;
                break;
            case ResourceValue.Half:
                tileColour = new Color(1.0f, 0.8f, 0.0f);
                break;
            case ResourceValue.Quarter:
                tileColour = new Color(1.0f, 0.5f, 0.0f);
                break;
            case ResourceValue.Min:
                tileColour = Color.white;
                break;
            case ResourceValue.Extracted:
                tileColour = Color.grey;
                break;
            default:
                tileColour = Color.white;
                break;
        }

        Tile.GetComponent<SpriteRenderer>().color = tileColour;
    }

    public void SetSurroundingTileResourceValues(ResourceValue value, bool roundUp)
    {
        bool lastPass = (value == ResourceValue.Min);

        // Foreach loop on list of surrounding tiles
        foreach (ResourceTile rTile in CloseTiles)
        {
            // If not on the last pass, also set values for other surrounding tiles
            if (!lastPass)
                rTile.SetSurroundingTileResourceValues(value + 1, roundUp);

            // Check if we should increase or decrease the value of the tiles
            bool changeValue = roundUp ? rTile.TileValue <= value : rTile.TileValue >= value;

            if (changeValue)
                rTile.TileValue = value;

            // REMOVE THIS AFTER TESTING, THIS WILL REVEAL ALL TILE VALUES
            rTile.UpdateTile();
        }
    }
}
