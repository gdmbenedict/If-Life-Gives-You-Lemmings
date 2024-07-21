using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToLemmings : MonoBehaviour
{
    [SerializeField] private Lemming lemmingComponent;
    [SerializeField] private Collider collider;
    private LemmingManager lemmingManager;

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
        //Debug.Log("Trigger Activated");

        if (other.transform.parent.GetComponent<PlayerController>())
        {
            //Debug.Log("Player Detected");
            lemmingManager.addLemming(lemmingComponent, gameObject.transform.parent.position.z);
            collider.isTrigger = false;
        }
    }
}
