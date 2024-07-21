using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    //connections
    GameManager gameManager;
    LemmingManager lemmingManager;
    ProgramManager programManager;

    [Header("Score values")]
    [SerializeField] private float okay = 15f;
    [SerializeField] private float great = 50f;
    [SerializeField] private float perfect = 100f;

    [Header("Modifier Values")]
    [SerializeField] private float lemmingModifier = 1.0f;
    [SerializeField] private float speedModifier = 1.0f;
    [SerializeField] private float comboModifier = 0.01f;

    private float score = 0;
    private int combo = 0;
   

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        lemmingManager = FindObjectOfType<LemmingManager>();
        programManager = FindObjectOfType<ProgramManager>();
    }

    // Update is called once per frame
    void Update()
    {
        programManager.UpdateScoreAndMult(score, CalcMult());
    }

    /// <summary>
    /// function that adds a given score
    /// </summary>
    /// <param name="baseValue"> the base amount of points added</param>
    private void AddScore(float baseValue)
    {
        score += baseValue * CalcMult();
        //Debug.Log("Score:" + score + " combo: x" + combo);
    }

    public float CalcMult()
    {
        return (lemmingModifier * lemmingManager.GetLemmings() + combo * comboModifier + gameManager.GetSpeed() * speedModifier);
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
