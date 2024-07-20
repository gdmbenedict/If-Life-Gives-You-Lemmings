using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [Header("Score values")]
    [SerializeField] private float okay = 15f;
    [SerializeField] private float great = 50f;
    [SerializeField] private float perfect = 100f;

    [Header("Modifier Values")]
    [SerializeField] private float lemmingModifier = 1.0f;
    [SerializeField] private float speedModifier = 1.0f;
    [SerializeField] private float comboModifier = 0.01f;

    private float score = 0;
    private int lemmings = 0;
    private int combo = 0;
   

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// function that adds a given score
    /// </summary>
    /// <param name="baseValue"> the base amount of points added</param>
    private void AddScore(float baseValue)
    {
        score += baseValue * (1 + lemmingModifier*lemmings + combo*comboModifier + 0*speedModifier);
        Debug.Log("Score:" + score + " combo: x" + combo);
    }

    public void Okay()
    {
        AddScore(okay);
    }

    public void Great()
    {
        AddScore(great);
    }

    public void Perfect()
    {
        AddScore(perfect);
    }

    public void AddCombo()
    {
        combo++;
    }

    public void ResetCombo()
    {
        combo = 0;
    }

    public float GetScore()
    {
        return score;
    }
}
