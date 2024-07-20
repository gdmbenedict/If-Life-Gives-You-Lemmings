using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    //game manager connection
    private GameManager gameManager;
    private TerrainManager terrainManager;

    //timing for input closness.
    [Header("Input Timings")]
    [SerializeField] private float perfectTiming = 0.045f;
    [SerializeField] private float greatTiming = 0.105f;
    [SerializeField] private float okayTiming = 0.180f;
    [SerializeField] private float clearDelay = 0.1f;

    [Header("Move Animation")]
    [SerializeField] private float hopHeight = 0.25f;

    private bool gotInput =false;
    private bool late = false;
    private bool autofail =false;
    private float timeSinceInput;
    private float recordedDirection;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        terrainManager = FindObjectOfType<TerrainManager>();
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
            Debug.Log("Got Input");
            recordedDirection = inputValue.Get<float>();

            //hit left border
            if (recordedDirection < 0 && transform.position.z >= (terrainManager.GetTileWidth()/2))
            {
                Debug.Log("Autofail called " + terrainManager.GetTileWidth() / 2);
                autofail = true;
            }
            //hit right border
            else if (recordedDirection > 0 && transform.position.z <= -(terrainManager.GetTileWidth()/ 2))
            {
                Debug.Log("Autofail called " + + -terrainManager.GetTileWidth() / 2);
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
            Debug.Log("okay timing: " + timeSinceInput);

            StartCoroutine(MoveAnimation(recordedDirection));
        }
        //great conditions
        else if (gotInput && timeSinceInput <= greatTiming && timeSinceInput > perfectTiming && !autofail)
        {
            Debug.Log("great timing: " + timeSinceInput);

            if (recordedDirection == 0)
            {
                //twirl
            }

            StartCoroutine(MoveAnimation(recordedDirection));
        }
        //perfect conditions
        else if (gotInput && timeSinceInput < perfectTiming && !autofail)
        {
            Debug.Log("perfect timing: " + timeSinceInput);

            if (recordedDirection == 0)
            {
                //twirl
            }

            StartCoroutine(MoveAnimation(recordedDirection));
        }
        //fail conditions
        else
        {
            Debug.Log("Failed Timing: " + timeSinceInput);
        }

        yield return new WaitForSeconds(perfectTiming);
        Invoke("ResetMovementParameters", clearDelay);
    }

    private IEnumerator MoveAnimation(float direction)
    {
        //get animation time
        float animTime = gameManager.GetAnimTime();
        StartCoroutine(HopAnim(animTime));

        //initial parameters
        float startPos = transform.position.z;
        float endPos = transform.position.z - direction;

        for(float t =0f; t< animTime; t += Time.deltaTime)
        {
            float newPos = Mathf.Lerp(startPos, endPos, t / animTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, newPos);
            yield return null;
        }

        //clamp to correct position
        transform.position = new Vector3(transform.position.x, transform.position.y, endPos);
    }

    private IEnumerator HopAnim(float animTime)
    {
        for (float t=0f; t<animTime; t+= Time.deltaTime)
        {
            float positionY = Mathf.Sin(Mathf.PI * t / animTime) * hopHeight;
            transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
            yield return null;
        }

        //clamp to correct position
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
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
}
