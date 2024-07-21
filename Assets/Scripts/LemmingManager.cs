using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LemmingManager : MonoBehaviour
{

    List<Lemming> lemmings;
    List<float> directionRegistry;

    private bool leaderDead = false;

    // Start is called before the first frame update
    void Start()
    {
        lemmings = new List<Lemming>();
        directionRegistry = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Method that logs the direction the source (player) moved
    /// </summary>
    /// <param name="direction"></param>
    public void LogDirection(float direction)
    {
        //add newest direction to registry
        directionRegistry.Add(-direction);

        //checks to see if registry is at least 1 larger than lemmings count
        if (!(directionRegistry.Count <= lemmings.Count))
        {
            //remove oldest direction in registry
            directionRegistry.Remove(directionRegistry[0]);
        }


    }

    /// <summary>
    /// Method that moves the lemmings in the same way the player moved
    /// </summary>
    public void moveLemmings()
    {
        for (int i =0; i<lemmings.Count; i++)
        {
            StartCoroutine(lemmings[i].MoveAnimation(directionRegistry[lemmings.Count-i-1]));
        }
    }

    public void addLemming(Lemming targetLemming, float currentPosZ)
    {
        //add to list
        lemmings.Add(targetLemming);

        //position lemming
        targetLemming.gameObject.transform.Rotate(0, 180, 0);
        
        float posZ = currentPosZ;
        foreach (float direction in directionRegistry)
        {
            posZ -= direction;
        }

        targetLemming.gameObject.transform.position = new Vector3(-lemmings.Count, 0, posZ);
        targetLemming.gameObject.transform.parent = gameObject.transform;
    }

    public void removeLemming(Lemming targetLemming)
    {
        if (!leaderDead)
        {
            int index = lemmings.IndexOf(targetLemming);

            if (index < lemmings.Count-1)
            {
                removeLemming(lemmings[index + 1]);
            }

            directionRegistry.Remove(directionRegistry[0]);
            lemmings.Remove(targetLemming);
            targetLemming.killLeming();
        }
    }

    public int GetLemmings()
    {
        return lemmings.Count;
    }

    public void KillLeader()
    {
        leaderDead = true;
    }
}
