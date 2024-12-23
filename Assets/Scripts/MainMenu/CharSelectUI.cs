using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectUI : BaseMenuUI
{
    public int maxLocalPlayers = 4;

    public List<CharacterRegistry> characters;
    
    public CharacterGridEntry entryTemplate;
    private List<CharacterGridEntry> entries;
    public Transform entryParent;
    public PlayerSelector selectorTemplate;
    public Transform selectorParent;
    public Transform playerReadyParent;
    public PlayerReadyIndicator playerReadyTemplate;
    private Dictionary<MainMenuPlayer, PlayerSelector> selectors;
    private List<PlayerReadyIndicator> indicators;
    [SerializeField]
    private AllReadyOverlay allReadyOverlay;

    protected override void Awake()
    {
        base.Awake();
        
        entries = new List<CharacterGridEntry>();
        selectors = new Dictionary<MainMenuPlayer, PlayerSelector>();
        indicators = new List<PlayerReadyIndicator>();

        // clear out any children left in scene??
        foreach (Transform child in entryParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in selectorParent)
        {
            Destroy(child.gameObject);
        }

        foreach (CharacterRegistry character in characters)
        {
            AddCharacter(character);
        }

        for (int i = 0; i < maxLocalPlayers; i++)
        {
            PlayerReadyIndicator indicator = Instantiate(playerReadyTemplate, playerReadyParent);
            indicator.Initialize(i + 1);
            indicators.Add(indicator);
        }
            
        allReadyOverlay.SetActive(false);
    }

    protected override void OnEnable()
    {
        foreach (PlayerSelector selector in selectors.Values)
        {
        }
    }

    public override void Navigate(MainMenuPlayer player, Vector2 input)
    {
        if (selectors.TryGetValue(player, out PlayerSelector selector))
        {
            if (selector.selectedCharacter != null && !selector.Locked)
            {
                Selectable nextButton = null;
                if (input.x > 0)
                {
                    nextButton = selector.selectedCharacter.Button.FindSelectableOnRight();
                }
                else if (input.x < 0)
                {
                    nextButton = selector.selectedCharacter.Button.FindSelectableOnLeft();
                }
                else if (input.y > 0)
                {
                    nextButton = selector.selectedCharacter.Button.FindSelectableOnUp();
                }
                else if (input.y < 0)
                {
                    nextButton = selector.selectedCharacter.Button.FindSelectableOnDown();
                }

                if (nextButton != null)
                {
                    CharacterGridEntry nextEntry = nextButton.GetComponent<CharacterGridEntry>();
                    if (nextEntry != null)
                    {
                        selector.selectedCharacter.RemoveSelector(selector);
                        selector.selectedCharacter = nextEntry;
                        selector.selectedCharacter.AddSelector(selector);
                    }    
                }
            }
        }
    }

    public override void Submit(MainMenuPlayer player)
    {
        if (selectors.TryGetValue(player, out PlayerSelector selector))
        {
            if (!selector.Locked)
            {
                selector.Lock();
                indicators[player.PlayerNum].Ready();
                
                UpdateReadyOverlay();
            }
        }
    }

    public override void Cancel(MainMenuPlayer player)
    {
        if (selectors.TryGetValue(player, out PlayerSelector selector))
        {
            if (selector.Locked)
            {
                selector.Unlock();
                indicators[player.PlayerNum].Unready();
            }
            else
            {
                RemovePlayer(player);
                indicators[player.PlayerNum].RemovePlayer();
            }
            
            UpdateReadyOverlay();
        }
    }

    public override void Confirm(MainMenuPlayer player)
    {
        if (player.PlayerNum == 0)
        {
            if (AllSelectorsLocked())
            {
                mainMenuUI.TransitionToMode(MenuMode.StageSelect);
            }
        }
    }

    private bool AllSelectorsLocked()
    {
        foreach (PlayerSelector selector in selectors.Values)
        {
            if (!selector.Locked)
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateReadyOverlay()
    {
        
        if (AllSelectorsLocked())
        {
            foreach (MainMenuPlayer player in selectors.Keys)
            {
                if (player.PlayerNum == 0)
                {
                    allReadyOverlay.SetControlScheme(player.ControlScheme);
                    break;
                }
            }            
            allReadyOverlay.SetActive(true);
        }
        else
        {
            allReadyOverlay.SetActive(false);
        }
    }

    private void AddCharacter(CharacterRegistry character)
    {
        CharacterGridEntry entry = Instantiate(entryTemplate, entryParent);
        entry.Initialize(character.displayName, character.icon);
        entries.Add(entry);
        if (firstSelectable == null)
        {
            firstSelectable = entry.Button;
        }
    }

    private void AssignToUniqueCharacter(PlayerSelector selector)
    {
        foreach (CharacterGridEntry entry in entries)
        {
            if (entry.PlayerCount == 0)
            {
                entry.AddSelector(selector);
                selector.selectedCharacter = entry;
                break;
            }
        }
    }

    public void AddPlayer(MainMenuPlayer player)
    {
        if (!selectors.ContainsKey(player))
        {
            PlayerSelector selector = Instantiate(selectorTemplate, selectorParent);
            selector.Initialize(player.PlayerNum + 1);
            selectors[player] = selector;

            
            if (player.ControlScheme == ControlScheme.Keyboard)
            {
                foreach (CharacterGridEntry entry in entries)
                {
                    entry.SetMouseSelector(selectors[player]);
                }
            }
        }

        foreach (PlayerReadyIndicator indicator in indicators)
        {
            if (indicator.IsFree)
            {
                indicator.AddPlayer(player.ControlScheme);
                break;
            }
        }

        AssignToUniqueCharacter(selectors[player]);
        UpdateReadyOverlay();
    }

    public void RemovePlayer(MainMenuPlayer player)
    {
        UpdateReadyOverlay();
    }
}