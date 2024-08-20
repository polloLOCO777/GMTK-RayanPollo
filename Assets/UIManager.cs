using System;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class MenuManager : Singleton<MenuManager>
{
    [Header("Main Menu")]
    [SerializeField] Menu mainMenu;

    [Header("Pause Menu")]
    [SerializeField] Menu pauseMenu;

    [Header("Win Menu")]
    [SerializeField] Menu winMenu;

    [Header("Lose Menu")]
    [SerializeField] Menu loseMenu;

    [Header("Loading Screen")]
    [SerializeField] Menu loadingScreen;

    [Header("Credits")]
    [SerializeField] Menu credits;

    [Header("Cameras")]
    [SerializeField] Camera userInterfaceCamera;

    public bool IsGamePaused { get; private set; }

    readonly Menu emptyMenu = new();
    Menu previousMenu;
    Menu currentMenu ;

    readonly List<Menu> menusToClear = new();

    KeyCode pauseKey = KeyCode.Escape;

    [Serializable]
    class Menu
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }
        [field: SerializeField] public List<GameObject> ObjectsToEnable { get; private set; }
        [field: SerializeField] public List<GameObject> ObjectsToDisable { get; private set; }
    }

    void Start()
    {
#if DEBUG
        pauseKey = KeyCode.P;
#endif
    }

    void OnEnable()
        => OnStateChangeEventHandler += HandleGameStateChange;

    void OnDisable()
        => OnStateChangeEventHandler -= HandleGameStateChange;

    private void Update()
        => PauseGame();

    /// <summary>
    ///     Loads the menu appropriate to the current game state.
    /// </summary>
    void HandleGameStateChange(object sender, StateChangeEventArgs e)
    {
        switch (e.newState)
        {
            case GameState.OpenGame:
                LoadMenu(mainMenu);
            break;
            
            case GameState.StartLevel:
            case GameState.RestartLevel:
                LoadMenu(emptyMenu);
            break;
            
            case GameState.LoseLevel:
                LoadMenu(loseMenu);
            break;
            
            case GameState.WinLevel:
                LoadMenu(winMenu);
            break;
        }
    }

    /// <summary>
    ///     Puase and unpause the game on escape press.
    /// </summary>
    void PauseGame()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (currentMenu == pauseMenu)
                LoadMenu(emptyMenu);
            else if (currentMenu == emptyMenu)
                LoadMenu(pauseMenu);
        }
    }

    /// <summary>
    ///     Loads the contents for a given menu while unloading the previous menu's.
    /// </summary>
    /// <param name="menu"> Menu to load. </param>
    void LoadMenu(Menu menu)
    {
        previousMenu = currentMenu;
        currentMenu = menu;
        
        // Ready current menu
        menu.Canvas.gameObject.SetActive(true);
        foreach (GameObject menuObject in menu.ObjectsToEnable)
            menuObject.SetActive(true);
        foreach (GameObject menuObject in menu.ObjectsToDisable)
            menuObject.SetActive(false);

        // Clear previous menu
        previousMenu.Canvas.gameObject.SetActive(false);
        foreach (GameObject menuObject in previousMenu.ObjectsToEnable)
            menuObject.SetActive(false);

        // Fill empty menu
        if (!menusToClear.Contains(menu))
        {
            foreach (GameObject menuObject in menu.ObjectsToEnable)
                emptyMenu.ObjectsToDisable.Add(menuObject);
            menusToClear.Add(menu);
        }
    }
}