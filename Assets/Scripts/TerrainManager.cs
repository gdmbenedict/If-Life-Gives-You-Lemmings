using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    //game manager connection
    private GameManager gameManager;

    [Header("Tile Info")]
    [SerializeField] private int tileWidth = 5;
    [SerializeField] private GameObject emptyTile;
    [SerializeField] private List<GameObject> tileVariants = new List<GameObject>();

    [Header("Track Management")]
    [SerializeField] private int trackLength = 24;
    [SerializeField] private int startPosX = 15;
    private GameObject[] activeTiles;
    private int counter = 0;
    bool movementDisabled = false;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

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

    public int GetTileWidth()
    {
        return tileWidth;
    }

    /// <summary>
    /// Method that picks a random tile from the tile variants list
    /// </summary>
    /// <returns>Random tile from the list of tile variants</returns>
    private GameObject pickRandomTile()
    {
        return tileVariants[UnityEngine.Random.Range(0, tileVariants.Count -1)];
    }

    /// <summary>
    /// Method that moves the track by one tile
    /// </summary>
    public void MoveTrack()
    {
        if (!movementDisabled)
        {
            for (int i = 1; i <= trackLength; i++)
            {
                GameObject targetTile = activeTiles[trackLength - i];

                //destroy last tile
                if (i == 1)
                {
                    Destroy(targetTile);
                }
                //move existing tile
                else
                {
                    activeTiles[trackLength - i + 1] = targetTile;
                    StartCoroutine(MoveTile(targetTile, targetTile.transform.position, targetTile.transform.position + Vector3.left));
                }
            }

            //allocate new tile
            GameObject newTile;

            //choose tile type
            if (counter <= 0)
            {
                newTile = Instantiate(pickRandomTile(), new Vector3(startPosX + 1, 0, 0), transform.rotation);
                counter = tileWidth;
            }
            else
            {
                newTile = Instantiate(emptyTile, new Vector3(startPosX + 1, 0, 0), transform.rotation);
            }

            //add Tile to list
            activeTiles[0] = newTile;
            counter--;

            //move into tile into frame
            StartCoroutine(MoveTile(newTile, newTile.transform.position, newTile.transform.position + Vector3.left));
        }    
    }

    /// <summary>
    /// Co-Routine that moves a track tile over a given amount of time
    /// </summary>
    /// <param name="target"></param>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    private IEnumerator MoveTile(GameObject target, Vector3 startPos, Vector3 endPos)
    {
        float moveTime = gameManager.GetAnimTime();

        for (float t = 0f; t < moveTime; t += Time.deltaTime)
        {
            target.transform.position = Vector3.Lerp(startPos, endPos, t/moveTime);
            yield return null;
        }

        //clamp to correct position
        target.transform.position = endPos;
    }

    public void DisableMovement()
    {
        movementDisabled = true;
    }
}
