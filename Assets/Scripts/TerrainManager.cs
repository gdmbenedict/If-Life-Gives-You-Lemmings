using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [Header("Tile Info")]
    [SerializeField] private int tileWidth = 5;
    [SerializeField] private GameObject emptyTile;
    [SerializeField] private List<GameObject> tileVariants = new List<GameObject>();

    [Header("Track Management")]
    [SerializeField] private int trackLength = 24;
    [SerializeField] private int startPosX = 15;
    private GameObject[] activeTiles;
    private int counter = 0;




    // Start is called before the first frame update
    void Start()
    {
        //initializing list of tiles
        activeTiles = new GameObject[trackLength];
        
        //populating with empty tiles
        for (int i =0; i < trackLength; i++)
        {
            GameObject newTile = Instantiate(emptyTile, new Vector3(startPosX - i, 0, 0), transform.rotation);
            activeTiles[i] = newTile;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Method that picks a random tile from the tile variants list
    /// </summary>
    /// <returns></returns>
    private GameObject pickRandomTile()
    {
        return tileVariants[UnityEngine.Random.Range(0, tileVariants.Count -1)];
    }
}
