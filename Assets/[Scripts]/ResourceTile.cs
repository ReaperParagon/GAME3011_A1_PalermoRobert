using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Header("Tile Information")]
    public GameObject Tile;
    public ResourceValue TileValue = ResourceValue.Min;
    public Vector2Int GridPosition = new Vector2Int(0, 0);
    public Color HiddenColour = Color.grey;

    public List<ResourceTile> CloseTiles = new List<ResourceTile>();

    public bool IsScanned = false;
    public bool Visited = false;

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
        TileValue = ResourceValue.Min;
        IsScanned = false;
        Visited = false;
        Tile.GetComponent<Image>().color = HiddenColour;
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
        IsScanned = true;
        UpdateTile();

        foreach (ResourceTile rTile in CloseTiles)
        {
            rTile.IsScanned = true;
            rTile.UpdateTile();
        }
    }

    private void OnExtract()
    {
        // Extract this tile's resources
        excavationManager.Extract(TileValue);
        SetResourceValue(ResourceValue.Min);

        // Reduce resources of surrounding 24 tiles
        foreach (ResourceTile rTile in CloseTiles)
        {
            rTile.DegradeCloseTiles();
        }

        excavationManager.ResetVisitedTiles();
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
                tileColour = Color.black;
                break;
            default:
                tileColour = Color.grey;
                break;
        }

        Tile.GetComponent<Image>().color = tileColour;
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

            // Check if we should change the value of this tile
            if (rTile.TileValue >= value)
                rTile.TileValue = value;
        }
    }

    private void DegradeCloseTiles()
    {
        foreach (ResourceTile rTile in CloseTiles)
        {
            rTile.DegradeTile();
        }
    }

    private void DegradeTile()
    {
        // Degrade value of this tile
        if (!Visited)
        {
            Visited = true;

            if (TileValue < ResourceValue.Min)
                TileValue += 1;

            if (IsScanned)
                UpdateTile();
        }
    }
}
