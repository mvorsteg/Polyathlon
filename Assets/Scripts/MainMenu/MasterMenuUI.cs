using System.Collections.Generic;
using UnityEngine;

public class MasterMenuUI : MonoBehaviour
{
    private MenuMode currentMode;
    private BaseMenuUI currentMenu;
    
    [SerializeField]
    private BaseMenuUI titleUI;
    [SerializeField]
    private BaseMenuUI charSelectUI;
    [SerializeField]
    private BaseMenuUI stageSelectUI;
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
        TransitionToMode(MenuMode.Title);
    }


    public void TransitionToMode(MenuMode newMode)
    {
        titleUI.gameObject.SetActive(false);
        charSelectUI.gameObject.SetActive(false);
        stageSelectUI.gameObject.SetActive(false);

        currentMode = newMode;
        switch (newMode)
        {
            case MenuMode.Title:
            {
                titleUI.gameObject.SetActive(true);
                currentMenu = titleUI;
            }
            break;
            case MenuMode.CharacterSelect:
            {
                charSelectUI.gameObject.SetActive(true);
                charSelectUI.Reset();
                foreach (MainMenuPlayer player in players)
                {
                    ((CharSelectUI)charSelectUI).AddPlayer(player);
                }
                currentMenu = charSelectUI;
            }
            break;
            case MenuMode.StageSelect:
            {
                stageSelectUI.gameObject.SetActive(true);
                stageSelectUI.Reset();
                currentMenu = stageSelectUI;
            }
            break;
            case MenuMode.Settings:
            {
                settingsUI.gameObject.SetActive(true);
                currentMenu = settingsUI;
            }
            break;
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