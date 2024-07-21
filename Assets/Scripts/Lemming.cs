using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Lemming : MonoBehaviour
{
    //connections
    GameManager gameManager;


    [Header("Move Animation")]
    [SerializeField] private float hopHeight = 0.25f;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void killLeming()
    {
        //death FX

        Destroy(gameObject);
    }

    public IEnumerator MoveAnimation(float direction)
    {
        //get animation time
        float animTime = gameManager.GetAnimTime();
        StartCoroutine(HopAnim(animTime));

        //initial parameters
        float startPos = transform.position.z;
        float endPos = transform.position.z - direction;

        for (float t = 0f; t < animTime; t += Time.deltaTime)
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
        for (float t = 0f; t < animTime; t += Time.deltaTime)
        {
            float positionY = Mathf.Sin(Mathf.PI * t / animTime) * hopHeight;
            transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
            yield return null;
        }

        //clamp to correct position
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    private IEnumerator MoveForward()
    {
        float moveTime = gameManager.GetAnimTime();
        float startX = gameObject.transform.position.x;
        float endX = startX + 1;

        for (float t = 0f; t < moveTime; t += Time.deltaTime)
        {
            float positionX = Mathf.Lerp(startX, endX, t/moveTime);
            transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
            yield return null;
        }


    }
}
