using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PolypediaUI : BaseMenuUI
{
    protected enum PolypediaState
    {
        TypeView,
        GridView,
        DetailsView,
    }

    [SerializeField]
    protected CharacterList characterList;
    [SerializeField]
    protected StageList stageList;
    [SerializeField]
    protected LootTable itemList;

    [SerializeField]
    protected Button backButton;

    [SerializeField]
    protected GameObject typeSelectView;
    [SerializeField]
    protected GameObject gridSelectView;
    [SerializeField]
    protected TextMeshProUGUI gridTitleText;
    [SerializeField]
    protected Transform gridParent;
    [SerializeField]
    protected GridEntry gridEntryTemplate;
    [SerializeField]
    private Selector selector;
    [SerializeField]
    protected GameObject detailsView;
    [SerializeField]
    protected TextMeshProUGUI detailsTitleText, detailsDescriptionText;
    [SerializeField]
    protected Image detailsIcon;
    protected PolypediaState currState;

    protected List<GridEntry> currentEntries;

    protected int currentDetailIdx = -1;

    protected override void Awake()
    {
        base.Awake();
        currState = PolypediaState.TypeView;
        selector.Initialize(0, "");
        currentEntries = new List<GridEntry>();
    }

    public override void Reset()
    {
        base.Reset();
        typeSelectView.SetActive(true);   
        gridSelectView.SetActive(false);   
        detailsView.SetActive(false);   
    }

    protected void SetState(PolypediaState newState)
    {        
        typeSelectView.SetActive(false);   
        gridSelectView.SetActive(false);   
        detailsView.SetActive(false);   
        switch (newState)
        {
            case PolypediaState.TypeView:
                {
                    typeSelectView.SetActive(true);   
                }
                break;
            case PolypediaState.GridView:
                {
                    gridSelectView.SetActive(true);   
                }
                break;
            case PolypediaState.DetailsView:
                {
                    detailsView.SetActive(true);   
                }
                break;

        }
        currState = newState;
    }

    public void BackButtonClicked()
    {
        switch (currState)
        {
            case PolypediaState.TypeView:
                {
                    mainMenuUI.TransitionToPreviousMode();
                }
                break;
            case PolypediaState.GridView:
            case PolypediaState.DetailsView:
                {
                    SetState(currState - 1);
                }
                break;
        } 
    }

    public void LeftArrowClicked()
    {
        
    }

    public void RightArrowClicked()
    {
        
    }

    public override void Navigate(MainMenuPlayer player, Vector2 input)
    {
        if (player.IsPrimary())
        {
            switch (currState)
            {
                case PolypediaState.TypeView:
                    {
                        base.Navigate(player, input);
                    }
                    break;
                case PolypediaState.GridView:
                    {
                        if (selector.selectedEntry != null && !selector.Locked)
                        {
                            UnityEngine.UI.Selectable nextButton = null;
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
                    break;
                case PolypediaState.DetailsView:
                    {
                        if (input.x != 0)
                        {
                            NavigateDetail((int)input.x);
                        }  
                    }
                    break;
            }
            
        }
    }

    public override void Submit(MainMenuPlayer player)
    {
        if (player.IsPrimary())
        {
            if (EventSystem.current.currentSelectedGameObject != backButton.gameObject)
                {
                switch (currState)
                {
                    case PolypediaState.TypeView:
                        {
                        }
                        break;
                    case PolypediaState.GridView:
                        {
                            PopulateDetailsView((IPolypediaEntry)selector.selectedEntry.Registry);
                            currentDetailIdx = selector.selectedEntry.transform.GetSiblingIndex();
                        }
                        break;
                    case PolypediaState.DetailsView:
                        {
                        }
                        break;

                }
            }
        }
    }

    public override void Cancel(MainMenuPlayer player)
    {
        if (player.IsPrimary())
        {
            BackButtonClicked();
        }
    }    
    
    public override void Confirm(MainMenuPlayer player)
    {
        // no difference here but want to allow same button press
        Submit(player);
    }    

    public void OnRacersClicked()
    {
        PopulateGridView(characterList.GetCharacters());
        gridTitleText.text = "Select a Character";
    }
    public void OnCoursesClicked()
    {
        PopulateGridView(stageList.stages);
        gridTitleText.text = "Select a Course";
    }
    public void OnItemsClicked()
    {
        PopulateGridView(itemList.GetAllItems());
        gridTitleText.text = "Select an Item";
    }

    protected void PopulateGridView<T>(IEnumerable<T> entries) where T : ScriptableObject, IPolypediaEntry
    {        
        SetState(PolypediaState.GridView);
        currentEntries.Clear();

        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        foreach (T entry in entries)
        {
            GridEntry gridEntry = Instantiate(gridEntryTemplate, gridParent);
            gridEntry.Initialize(entry, entry.DisplayName, entry.Thumbnail, 1);
            if (mainMenuUI.PrimaryControlScheme == ControlScheme.Keyboard)
            {
                gridEntry.SetMouseSelector(selector);
            }
            currentEntries.Add(gridEntry);
        }

        if (currentEntries.Count > 0)
        {
            currentEntries[0].AddSelector(selector, true);
            selector.selectedEntry = currentEntries[0];
        }
    }

    protected void PopulateDetailsView(IPolypediaEntry entry)
    {
        SetState(PolypediaState.DetailsView);  

        detailsTitleText.text = entry.DisplayName;
        detailsDescriptionText.text = entry.Description;
        if (entry.Slides.Count > 0)
        {
            detailsIcon.sprite = entry.Slides[0];
        }
        else
        {
            detailsIcon.sprite = entry.Thumbnail;
        }
        detailsIcon.preserveAspect = true;
    }

    public void NavigateDetail(int dir)
    {
        currentDetailIdx = currentDetailIdx + dir;
        if (currentDetailIdx < 0)
        {
            currentDetailIdx = gridParent.childCount - 1;
        }
        if (currentDetailIdx >= gridParent.childCount)
        {
            currentDetailIdx = 0;
        }
        GridEntry nextEntry = gridParent.GetChild(currentDetailIdx).GetComponent<GridEntry>();
        if (nextEntry != null)
        {
            PopulateDetailsView((IPolypediaEntry)nextEntry.Registry);
        }
    }

}