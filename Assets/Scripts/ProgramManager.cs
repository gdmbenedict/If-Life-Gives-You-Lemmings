using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    //references
    [Header("UI Objects")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject gameplayUI;

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
        mainMenu.SetActive(true);
        menuMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
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
