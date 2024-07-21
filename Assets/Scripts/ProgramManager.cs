using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgramManager : MonoBehaviour
{
    public enum GameState
    {
        mainMenu,
        gamePlay
    }

    public enum UIState
    {
        mainMenu,
        pauseMenu,
        optionsMenu,
        creditsMenu,
        endScreen,
        gamePlay,
        controlsMenu
    }

    //State enums
    private GameState gameState;
    private UIState uiState;

    //object connections
    private GameManager gameManager;
    private Score score;

    //references
    [Header("UI Objects")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject gameplayUI;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [SerializeField] private TextMeshProUGUI multDisplay;
    [SerializeField] private Image image3;
    [SerializeField] private Image image2;
    [SerializeField] private Image image1;
    [SerializeField] private Image imageGo;
    [SerializeField] private Image perfectImage;
    [SerializeField] private Image greatImage;
    [SerializeField] private Image okayImage;

    //Scenes
    [Header("Scenes")]
    [SerializeField] private string mainMenuScene;
    [SerializeField] private string gameplayScene;

    [Header("Menu Audio")]
    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private AudioSource MenuSFX;

    // Start is called before the first frame update
    void Start()
    {
        image1.enabled = false;
        image2.enabled = false;
        image3.enabled = false;
        imageGo.enabled = false;

        mainMenu.SetActive(true);
        menuMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScoreAndMult(float score, float mult)
    {
        if (gameState == GameState.gamePlay && gameManager != null)
        {
            scoreDisplay.text = "Score: " + score.ToString("F0");
            multDisplay.text = "Multipler: " + mult.ToString("F2");
        }
    }

    public IEnumerator StartSequence(GameManager gameManager)
    {
        this.gameManager = gameManager;
        float animTime = gameManager.GetAnimTime()*2;

        image3.enabled = true;
        for (float t=0; t<= animTime; t+= Time.deltaTime)
        {
            image3.color = new Color(1, 1, 1, t/animTime);
            yield return null;
        }
        image3.enabled = false;

        image2.enabled = true;
        for (float t = 0; t <= animTime; t += Time.deltaTime)
        {
            image2.color = new Color(1, 1, 1, t / animTime);
            yield return null;
        }
        image2.enabled = false;

        image1.enabled = true;
        for (float t = 0; t <= animTime; t += Time.deltaTime)
        {
            image1.color = new Color(1, 1, 1, t / animTime);
            yield return null;
        }
        image1.enabled = false;

        imageGo.enabled = true;
        gameManager.StartGame();
        for (float t = 0; t <= animTime; t += Time.deltaTime)
        {
            imageGo.color = new Color(1, 1, 1, t / animTime);
            yield return null;
        }
        imageGo.enabled = false;   
    }

    public IEnumerator Perfect()
    {
        float animTime = gameManager.GetAnimTime();
        perfectImage.enabled = true;
        for (float t = 0; t <= animTime; t += Time.deltaTime)
        {
            perfectImage.color = new Color(1, 1, 1, t / animTime);
            yield return null;
        }
        perfectImage.enabled = false;
    }

    public IEnumerator Great()
    {
        float animTime = gameManager.GetAnimTime();
        greatImage.enabled = true;
        for (float t = 0; t <= animTime; t += Time.deltaTime)
        {
            greatImage.color = new Color(1, 1, 1, t / animTime);
            yield return null;
        }
        greatImage.enabled = false;
    }

    public IEnumerator Okay()
    {
        float animTime = gameManager.GetAnimTime();
        okayImage.enabled = true;
        for (float t = 0; t <= animTime; t += Time.deltaTime)
        {
            okayImage.color = new Color(1, 1, 1, t / animTime);
            yield return null;
        }
        okayImage.enabled = false;
    }

    public void OnPause(InputValue inputValue)
    {
        Back();
    }

    public void Back()
    {
        switch (uiState)
        {
            case UIState.mainMenu:
                Quit();
                break;

            case UIState.pauseMenu:
                TogglePause();
                break;

            case UIState.optionsMenu:

                optionsMenu.SetActive(false);

                if (gameState == GameState.mainMenu)
                {
                    mainMenu.SetActive(true);
                    uiState = UIState.mainMenu;
                }
                else
                {
                    pauseMenu.SetActive(true);
                    uiState = UIState.pauseMenu;
                }

                break;

            case UIState.creditsMenu:

                creditsMenu.SetActive(false);

                if (gameState == GameState.mainMenu)
                {
                    mainMenu.SetActive(true);
                    uiState = UIState.mainMenu;
                }
                else
                {
                    pauseMenu.SetActive(true);
                    uiState = UIState.pauseMenu;
                }

                break;

            case UIState.gamePlay:
                TogglePause();
                break;

            case UIState.endScreen:
                ChangeScene(mainMenuScene);
                break;

            case UIState.controlsMenu:

                controlsMenu.SetActive(false);

                if (gameState == GameState.mainMenu)
                {
                    mainMenu.SetActive(true);
                    uiState = UIState.mainMenu;
                }
                else
                {
                    pauseMenu.SetActive(true);
                    uiState = UIState.pauseMenu;
                }

                break;
        }
    }

    public void OpenMenu(string desiredMenu)
    {
        switch (desiredMenu)
        {
            case "optionsMenu":

                if (gameState == GameState.mainMenu)
                {
                    mainMenu.SetActive(false);
                }
                else
                {
                    pauseMenu.SetActive(false);
                }

                optionsMenu.SetActive(true);
                uiState = UIState.optionsMenu;

                break;

            case "creditsMenu":

                if (gameState == GameState.mainMenu)
                {
                    mainMenu.SetActive(false);
                }
                else
                {
                    pauseMenu.SetActive(false);
                }

                creditsMenu.SetActive(true);
                uiState = UIState.creditsMenu;

                break;

            case "controlsMenu":

                if (gameState == GameState.mainMenu)
                {
                    mainMenu.SetActive(false);
                }
                else
                {
                    pauseMenu.SetActive(false);
                }

                controlsMenu.SetActive(true);
                uiState = UIState.controlsMenu;

                break;

            case "endScreen":

                scoreText.text = "Score: " + FindObjectOfType<Score>().GetScore();

                gameplayUI.SetActive(false);
                endScreen.SetActive(true);

                break;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void TogglePause()
    {
        FindObjectOfType<GameManager>().TogglePause();

        if (Time.timeScale == 0)
        {
            pauseMenu.SetActive(true);
            menuMusic.Play();
            uiState = UIState.pauseMenu;
        }
        else
        {
            pauseMenu.SetActive(false);
            menuMusic.Stop();
            uiState = UIState.gamePlay;
        }
        
    }

    public void ChangeScene(string sceneName)
    {
        endScreen.SetActive(false);

        SceneManager.LoadScene(sceneName);

        if (sceneName == mainMenuScene)
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }

            pauseMenu.SetActive(false);
            endScreen.SetActive(false);
            mainMenu.SetActive(true);
            gameState = GameState.mainMenu;
            uiState = UIState.mainMenu;

            SceneManager.LoadScene(mainMenuScene);
            menuMusic.Play();
        }
        else
        {
            mainMenu.SetActive(false);
            gameState = GameState.gamePlay;
            uiState = UIState.gamePlay;
            gameplayUI.SetActive(true);

            SceneManager.LoadScene(gameplayScene);
            menuMusic.Stop();
        }
    }
}
