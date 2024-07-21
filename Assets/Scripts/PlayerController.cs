using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    //manager connections
    private GameManager gameManager;
    private TerrainManager terrainManager;
    private Score score;
    private LemmingManager lemmingManager;
    [SerializeField] private Lemming lemmingComponent;

    //timing for input closness.
    [Header("Input Timings")]
    [SerializeField] private float basePerfectTiming = 0.045f;
    [SerializeField] private float baseGreatTiming = 0.105f;
    [SerializeField] private float baseOkayTiming = 0.180f;
    [SerializeField] private float baseClearDelay = 0.1f;

    private float perfectTiming;
    private float greatTiming;
    private float okayTiming;
    private float clearDelay;

    private bool gotInput =false;
    private bool late = false;
    private bool autofail =false;
    private float timeSinceInput;
    private float recordedDirection;

    private bool isDead = false;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        terrainManager = FindObjectOfType<TerrainManager>();
        score = FindAnyObjectByType<Score>();
        lemmingManager = FindObjectOfType<LemmingManager>();

        calcTimings(gameManager.GetSpeed());
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //increment time since input
        timeSinceInput += Time.deltaTime;
    }

    private void RecordInputTime()
    {
        //record that input was taken and reset timer
        gotInput = true;

        if (!late)
        {
            //Debug.Log("Resseting time since input");
            timeSinceInput = 0f;
        }   
    }

    public void OnMovement(InputValue inputValue)
    {
        if (!gotInput)
        {
            //Debug.Log("Got Input");
            recordedDirection = inputValue.Get<float>();

            //hit left border
            if (recordedDirection < 0 && transform.position.z >= (terrainManager.GetTileWidth()/2))
            {
                //Debug.Log("Autofail called " + terrainManager.GetTileWidth() / 2);
                autofail = true;
            }
            //hit right border
            else if (recordedDirection > 0 && transform.position.z <= -(terrainManager.GetTileWidth()/ 2))
            {
                //Debug.Log("Autofail called " + + -terrainManager.GetTileWidth() / 2);
                autofail =true;
            }

            RecordInputTime();
        }
    }

    public void OnFlourish(InputValue inputValue)
    {
        if (!gotInput)
        {
            //Debug.Log("Got Input");
            recordedDirection = 0;
            RecordInputTime();
        }
    }

    public void MovePlayer()
    {
        //Debug.Log("Move Player Called");
        StartCoroutine(Movefunction());
    }

    /// <summary>
    /// method that resets the movement parameters for the next move
    /// </summary>
    private void ResetMovementParameters()
    {
        //Debug.Log("Resetting parameters");

        recordedDirection = 0;
        gotInput = false;
        late = false;
        autofail = false;
    }

    public void pulse()
    {
        StartCoroutine(popAnim());
    }
    
    private IEnumerator Movefunction()
    {
        if (!isDead)
        {
            if (!gotInput)
            {
                //Debug.Log("waiting for input");
                late = true;
                timeSinceInput = 0f;
                while (!gotInput && timeSinceInput < okayTiming)
                {
                    yield return null;
                }
            }

            //okay conditions
            if (gotInput && timeSinceInput <= okayTiming && timeSinceInput > greatTiming && recordedDirection != 0 && !autofail)
            {
                //Debug.Log("okay timing: " + timeSinceInput);

                //add score
                score.Okay();
                score.AddCombo();

                //move animation
                StartCoroutine(lemmingComponent.MoveAnimation(recordedDirection));
            }
            //great conditions
            else if (gotInput && timeSinceInput <= greatTiming && timeSinceInput > perfectTiming && !autofail)
            {
                //Debug.Log("great timing: " + timeSinceInput);

                if (recordedDirection == 0)
                {
                    //twirl
                }

                //add score
                score.Great();
                score.AddCombo();

                //move animation
                StartCoroutine(lemmingComponent.MoveAnimation(recordedDirection));
            }
            //perfect conditions
            else if (gotInput && timeSinceInput < perfectTiming && !autofail)
            {
                //Debug.Log("perfect timing: " + timeSinceInput);

                if (recordedDirection == 0)
                {
                    //twirl
                }

                //add score
                score.Perfect();
                score.AddCombo();

                //move animation
                StartCoroutine(lemmingComponent.MoveAnimation(recordedDirection));

            }
            //fail conditions
            else
            {
                score.ResetCombo();
                recordedDirection = 0;
                //Debug.Log("Failed Timing: " + timeSinceInput);
            }
        }

        lemmingManager.LogDirection(recordedDirection);
        lemmingManager.moveLemmings();
        Invoke("ResetMovementParameters", clearDelay);
    }
    
    private IEnumerator popAnim()
    {
        float animTime = gameManager.GetAnimTime();
        Vector3 originalScale = transform.localScale;

        Vector3 popScale = new Vector3(transform.localScale.x * 1.25f, transform.localScale.y * 1.25f, transform.localScale.z * 1.25f);

        for (float t=0f; t<animTime/2; t+= Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(popScale, originalScale, t / (animTime / 2));
            yield return null;
        }

        //snap to correct value
        transform.localScale = originalScale;
    }

    public void calcTimings(float speedmodifier)
    {
        perfectTiming = basePerfectTiming / speedmodifier;
        greatTiming = baseGreatTiming / speedmodifier;
        okayTiming = baseOkayTiming / speedmodifier;
        clearDelay = baseClearDelay / speedmodifier;
    }

    public void killPlayer()
    {
        isDead = true;

        //disable visualization
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponent<PlayerInput>().enabled = false;
        terrainManager.DisableMovement();
    }
}
