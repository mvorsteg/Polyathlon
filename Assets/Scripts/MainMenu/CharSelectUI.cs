using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectUI : BaseMenuUI
{
    public int maxLocalPlayers = 4;

    public List<CharacterRegistry> characters;
    
    public GridEntry entryTemplate;
    private List<GridEntry> entries;
    public Transform entryParent;
    public Selector selectorTemplate;
    public Transform selectorParent;
    public Transform playerReadyParent;
    public PlayerReadyIndicator playerReadyTemplate;
    //private Dictionary<MainMenuPlayer, BaseSelector> selectors;
    private List<Selector> selectors;
    private List<PlayerReadyIndicator> indicators;
    [SerializeField]
    private AllReadyOverlay allReadyOverlay;

    protected override void Awake()
    {
        base.Awake();
        
        entries = new List<GridEntry>();
        selectors = new List<Selector>();
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
            Selector selector = Instantiate(selectorTemplate, selectorParent);
            selector.Initialize(i + 1, string.Format("P{0}", i + 1));
            selector.SetActive(false);
            selectors.Add(selector);

            PlayerReadyIndicator indicator = Instantiate(playerReadyTemplate, playerReadyParent);
            indicator.Initialize(i + 1);
            indicators.Add(indicator);
        }
            
        allReadyOverlay.SetActive(false);
    }

    protected override void OnEnable()
    {
        
    }

    public override void Reset()
    {
        base.Reset();
        
        allReadyOverlay.SetActive(false);
        foreach (Selector selector in selectors)
        {
            selector.Unlock();
            selector.SetActive(false);
        }

        foreach (GridEntry entry in entries)
        {
            entry.Reset();
        }
    }

    public override void Navigate(MainMenuPlayer player, Vector2 input)
    {
        Selector selector = GetSelectorForPlayer(player);
        if (selector != null)
        {
            if (selector.selectedEntry != null && !selector.Locked)
            {
                Selectable nextButton = null;
                if (input.x > 0)
                {
                    nextButton = selector.selectedEntry.Button.FindSelectableOnRight();
                }
                else if (input.x < 0)
                {
                    nextButton = selector.selectedEntry.Button.FindSelectableOnLeft();
                }
                else if (input.y > 0)
                {
                    nextButton = selector.selectedEntry.Button.FindSelectableOnUp();
                }
                else if (input.y < 0)
                {
                    nextButton = selector.selectedEntry.Button.FindSelectableOnDown();
                }

                if (nextButton != null)
                {
                    GridEntry nextEntry = nextButton.GetComponent<GridEntry>();
                    if (nextEntry != null)
                    {
                        selector.selectedEntry.RemoveSelector(selector);
                        selector.selectedEntry = nextEntry;
                        selector.selectedEntry.AddSelector(selector, false);
                    }    
                }
            }
        }
    }

    public override void Submit(MainMenuPlayer player)
    {
        Selector selector = GetSelectorForPlayer(player);
        if (selector != null)
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
        Selector selector = GetSelectorForPlayer(player);
        if (selector != null)
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
            
            bool allPlayersLeft = true;
            foreach (PlayerReadyIndicator indicator in indicators)
            {
                if (!indicator.IsFree)
                {
                    allPlayersLeft = false;
                    break;
                }
            }
            if (allPlayersLeft)
            {
                mainMenuUI.TransitionToMode(MenuMode.Title);
            }
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
        foreach (Selector selector in selectors)
        {
            if (selector.Active && !selector.Locked)
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
            allReadyOverlay.SetControlScheme(mainMenuUI.PrimaryControlScheme);
            allReadyOverlay.SetActive(true);
        }
        else
        {
            allReadyOverlay.SetActive(false);
        }
    }

    private void AddCharacter(CharacterRegistry character)
    {
        GridEntry entry = Instantiate(entryTemplate, entryParent);
        entry.Initialize(character.displayName, character.icon, maxLocalPlayers);
        entries.Add(entry);
        if (firstSelectable == null)
        {
            firstSelectable = entry.Button;
        }
    }

    public void AddPlayer(MainMenuPlayer player)
    {
        Selector selector = GetSelectorForPlayer(player);
        if (selector != null)
        {
            selector.SetActive(true);
            if (player.ControlScheme == ControlScheme.Keyboard)
            {
                foreach (GridEntry entry in entries)
                {
                    entry.SetMouseSelector(GetSelectorForPlayer(player));
                }
            }
            
            // assign to unique character
            foreach (GridEntry entry in entries)
            {
                if (entry.SelectorCount == 0)
                {
                    entry.AddSelector(selector, true);
                    selector.selectedEntry = entry;
                    break;
                }
            
            }
        }

        PlayerReadyIndicator indicator = GetIndicatorForPlayer(player);
        {
            if (indicator.IsFree)
            {
                indicator.AddPlayer(player.ControlScheme);
            }
        }

        UpdateReadyOverlay();
    }

    public void RemovePlayer(MainMenuPlayer player)
    {
        UpdateReadyOverlay();
        Selector selector = GetSelectorForPlayer(player);
        if (selector != null)
        {
            selector.SetActive(false);
        }
    }

    private Selector GetSelectorForPlayer(MainMenuPlayer player)
    {
        if (player.PlayerNum >= 0 && player.PlayerNum < maxLocalPlayers)
        {
            return selectors[player.PlayerNum];
        }
        return null;
    }

    private PlayerReadyIndicator GetIndicatorForPlayer(MainMenuPlayer player)
    {
        if (player.PlayerNum >= 0 && player.PlayerNum < maxLocalPlayers)
        {
            return indicators[player.PlayerNum];
        }
        return null;
    }
}