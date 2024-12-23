using System.Collections.Generic;
using UnityEngine;

public class StageSelectUI : BaseMenuUI
{
    
    public List<StageRegistry> stages;
    public StageGridEntry entryTemplate;
    private List<StageGridEntry> entries;
    public Transform entryParent;
    [SerializeField]
    private StageSelector selector;

    protected override void Awake()
    {
        base.Awake();

        entries = new List<StageGridEntry>();

        // clear out any children left in scene??
        foreach (Transform child in entryParent)
        {
            Destroy(child.gameObject);
        }

        foreach (StageRegistry stage in stages)
        {
            AddStage(stage);
        }
    }

    public override void Submit(MainMenuPlayer player)
    {
        if (player.PlayerNum == 0)
        {
            if (!selector.Locked)
            {
                selector.Lock();
                                
                //UpdateReadyOverlay();
            }
        }
    }

    public override void Cancel(MainMenuPlayer player)
    {
        if (player.PlayerNum == 0)
        {
            if (selector.Locked)
            {
                selector.Unlock();
            }
            
            //UpdateReadyOverlay();
        }
    }

    public override void Confirm(MainMenuPlayer player)
    {
        if (player.PlayerNum == 0)
        {
            if (selector.Locked)
            {
                mainMenuUI.TransitionToMode(MenuMode.StageSelect);
            }
        }
    }

    private void AddStage(StageRegistry stage)
    {
        StageGridEntry entry = Instantiate(entryTemplate, entryParent);
        entry.Initialize(stage.displayName, stage.icon);
        entries.Add(entry);
        if (firstSelectable == null)
        {
            firstSelectable = entry.Button;
        }
    }
}