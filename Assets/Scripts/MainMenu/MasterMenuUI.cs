using System.Collections.Generic;
using UnityEngine;

public class MasterMenuUI : MonoBehaviour
{
    private MenuMode currentMode;
    private BaseMenuUI currentMenu;
    private Stack<MenuMode> previousMenus;
    
    [SerializeField]
    private BaseMenuUI titleUI;
    [SerializeField]
    private BaseMenuUI charSelectUI;
    [SerializeField]
    private BaseMenuUI raceSettingsUI;
    [SerializeField]
    private BaseMenuUI stageSelectUI;
    [SerializeField]
    private BaseMenuUI trainingSelectUI;
    [SerializeField]
    private BaseMenuUI settingsUI;
    
    private bool inputSchemeRegistered = false;
    public List<MainMenuPlayer> players;

    public ControlScheme PrimaryControlScheme { get; private set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        previousMenus = new Stack<MenuMode>();

        currentMode = MenuMode.Invalid;
        TransitionToMode(MenuMode.Title);
    }


    public void TransitionToMode(MenuMode newMode)
    {
        titleUI.gameObject.SetActive(false);
        charSelectUI.gameObject.SetActive(false);
        raceSettingsUI.gameObject.SetActive(false);
        stageSelectUI.gameObject.SetActive(false);
        trainingSelectUI.gameObject.SetActive(false);
        settingsUI.gameObject.SetActive(false);

        if (currentMode != MenuMode.Invalid)
        {
            previousMenus.Push(currentMode);
        }

        currentMode = newMode;
        switch (newMode)
        {
            case MenuMode.Title:
            {
                currentMenu = titleUI;
            }
            break;
            case MenuMode.CharacterSelect:
            {
                currentMenu = charSelectUI;
            }
            break;
            case MenuMode.RaceSettings:
            {
                currentMenu = raceSettingsUI;
            }
            break;
            case MenuMode.StageSelect:
            {
                currentMenu = stageSelectUI;
            }
            break;
            case MenuMode.TrainingSelect:
            {
                currentMenu = trainingSelectUI;
            }
            break;
            case MenuMode.Settings:
            {
                currentMenu = settingsUI;
            }
            break;
        }

        currentMenu.gameObject.SetActive(true);
        currentMenu.Reset();
        
        if (currentMenu is CharSelectUI charMenu)
        {
            foreach (MainMenuPlayer player in players)
            {
                charMenu.AddPlayer(player);
            }
        }
    }

    public void TransitionToPreviousMode()
    {
        if (previousMenus.Count > 0)
        {
            MenuMode prevMode = previousMenus.Pop();
            currentMode = MenuMode.Invalid;
            TransitionToMode(prevMode);
        }
    }

    public virtual void AnyKeyPressed(ControlScheme scheme)
    {
        if (!inputSchemeRegistered)
        {
            PrimaryControlScheme = scheme;
            inputSchemeRegistered = true;
        }

        currentMenu.AnyKeyPressed();
    }

    public void AddPlayer(MainMenuPlayer player, ControlScheme scheme)
    {
        AnyKeyPressed(scheme);
        players.Add(player);
        if (currentMode == MenuMode.CharacterSelect)
        {
            ((CharSelectUI)charSelectUI).AddPlayer(player);
        }
    }

    public void Navigate(MainMenuPlayer player, Vector2 input)
    {
        // make stick input discretely vertical or horizontal
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            input.y = 0;
        }
        else
        {
            input.x = 0;
        }
        
        currentMenu.Navigate(player, input);
    }

    public void Submit(MainMenuPlayer player)
    {
        currentMenu.Submit(player);
    }

    public void Cancel(MainMenuPlayer player)
    {
        currentMenu.Cancel(player);
    }

    public void Confirm(MainMenuPlayer player)
    {
        currentMenu.Confirm(player);
    }

}