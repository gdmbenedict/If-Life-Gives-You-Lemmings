using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    LemmingManager lemmingManager;

    // Start is called before the first frame update
    void Start()
    {
        lemmingManager = FindObjectOfType<LemmingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Called");

        Lemming lemmingComponent = other.transform.parent.GetComponent<Lemming>();

        if (lemmingComponent != null)
        {
            Debug.Log("Lemming component found");
            lemmingManager.removeLemming(lemmingComponent);
        }
    }
}
