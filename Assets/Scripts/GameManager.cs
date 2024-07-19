using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Management")]
    [SerializeField] private TerrainManager terrainManager;

    [Header("Rythm Management")]
    [SerializeField] private int tempo = 120;
    private float secPerBeat;
    private float timer =0f;

    // Start is called before the first frame update
    void Start()
    {
        secPerBeat = 1f / (tempo / 60f);
        terrainManager.SetMoveTime(secPerBeat);
    }

    // Update is called once per frame
    void Update()
    {
        //update timer
        timer += Time.deltaTime;

        //call turn
        if (timer >= secPerBeat*2)
        {
            PlayTurn();
            timer = 0f;
        }
    }

    /// <summary>
    /// Method that initiates the turn
    /// </summary>
    private void PlayTurn()
    {
        terrainManager.MoveTrack();
    }
}
