using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Have a private List of Grid Tiles

    [Header("Grid Tile Information")]
    [SerializeField]
    private GameObject GridTilePrefab;
    [SerializeField]
    private int extractionAttempts = 3;
    [SerializeField]
    private int scanAttempts = 6;
    [SerializeField]
    private int numMaxResourceTiles = 40;

    // When player starts extraction:
    // Generate Max Resources in unique tiles
    // Generate the surrounding sub resources around each max resource immediately
    // When generating sub resources, check if the tile's resource quality is better than the one being placed

    private void Awake()
    {
        // Setup Grid Layout
        gridLayout = GridArea.GetComponent<GridLayoutGroup>();
        gridLayout.constraintCount = GridDimensions.x;

        for (int i = 0; i < GridCount; i++)
        {
            // Add Tile to Grid
            Instantiate(GridTilePrefab, GridArea.transform);

            // Store info based on all grid positions
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
