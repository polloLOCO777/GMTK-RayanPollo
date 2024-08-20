using System;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class MenuManager : Singleton<MenuManager>
{
    [Header("Main Menu")]
    [SerializeField] Menu mainMenu;

    [Header("Credits")]
    [SerializeField] Menu credits;

    [Header("Pause Menu")]
    [SerializeField] Menu pauseMenu;

    [Header("Credits Menu")]
    [SerializeField] Menu creditsMenu;

    [Header("End Menu")]
    [SerializeField] Menu endMenu;

    [Header("Cameras")]
    [SerializeField] Camera userInterfaceCamera;

    public bool IsGamePaused { get; private set; }

    readonly Menu emptyMenu = new();

    Menu previousMenu; // Update to be more sophisticated
    Menu currentMenu;

    readonly List<Menu> menusToClear = new();
    readonly List<Menu> menuHistory = new();
    int currentHistoryIndex = 0;

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
            
            case GameState.WinLevel:
                LoadMenu(endMenu);
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
    void LoadMenu(Menu menu, bool addToHistory = true)
    {
        menuHistory.Add(menu);
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

    /// <summary>
    /// Loads the menu of the previous index in the menu history
    /// </summary>
    void PreviousMenu()
    {
        if (currentHistoryIndex == 0)
            return;

        if (currentHistoryIndex >= menuHistory.Count)
            throw new Exception("Index out of bounds");

        menuHistory.RemoveAt(currentHistoryIndex);
        
        // -2 to account for removing an item and going back an item
        currentHistoryIndex--;
        LoadMenu(menuHistory[currentHistoryIndex--], false);
    }
}