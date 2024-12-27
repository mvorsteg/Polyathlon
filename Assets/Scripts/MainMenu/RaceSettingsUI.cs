using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaceSettingsUI : BaseMenuUI
{
    [SerializeField]
    private Spinner raceNumSpinner, raceSelectSpinner, cpuDiffSpinner, cpuNumSpinner;
    [SerializeField]
    private AllReadyOverlay allReadyOverlay;
    [SerializeField]
    private StageSelectUI stageSelectUI;
    public int maxTotalRacers = 12;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Reset()
    {
        int maxCPURacers = maxTotalRacers - raceSettings.PlayerChoices.Count;
        cpuNumSpinner.ClearValues();
        for (int i = 1; i <= maxCPURacers; i++)
        {
            cpuNumSpinner.AddValue(i.ToString());
        }
        
        allReadyOverlay.SetActive(false);
    }

    public override void Navigate(MainMenuPlayer player, Vector2 input)
    {
        base.Navigate(player, input);

        if (input.x != 0 && player.PlayerNum == 0)
        {
            if (EventSystem.current.currentSelectedGameObject == raceNumSpinner.gameObject)
            {
                raceNumSpinner.Navigate(input.x > 0);
            }
            else if (EventSystem.current.currentSelectedGameObject == raceSelectSpinner.gameObject)
            {
                raceSelectSpinner.Navigate(input.x > 0);
            }
            else if (EventSystem.current.currentSelectedGameObject == cpuDiffSpinner.gameObject)
            {
                cpuDiffSpinner.Navigate(input.x > 0);
            }
            else if (EventSystem.current.currentSelectedGameObject == cpuNumSpinner.gameObject)
            {
                cpuNumSpinner.Navigate(input.x > 0);
            }
        }
    }

    public void ApplySettings()
    {
        if (int.TryParse(raceNumSpinner.Value, out int numRaces) &&
            //Enum.TryParse(raceSelectSpinner.Value, out RaceSelection raceSelection) &&
            EnumUtility.TryGetValueFromDescription(raceSelectSpinner.Value, out RaceSelection raceSelection) &&
            Enum.TryParse(cpuDiffSpinner.Value, out CPUDifficulty cpuDifficulty) &&
            int.TryParse(cpuNumSpinner.Value, out int numCPUs))
            {
                raceSettings.SetRaceParams(numRaces, raceSelection, cpuDifficulty, numCPUs);
                
                if (raceSelection == RaceSelection.Random)
                {
                    raceSettings.PreloadStages(stageSelectUI.GetNStages(numRaces, true));
                    allReadyOverlay.SetControlScheme(mainMenuUI.PrimaryControlScheme);
                    allReadyOverlay.SetActive(true);
                }
                else if (raceSelection == RaceSelection.InOrder)
                {
                    raceSettings.PreloadStages(stageSelectUI.GetNStages(numRaces, false));
                    allReadyOverlay.SetControlScheme(mainMenuUI.PrimaryControlScheme);
                    allReadyOverlay.SetActive(true);
                }
                else
                {
                    mainMenuUI.TransitionToMode(MenuMode.StageSelect);
                }
            }

    }

    public override void Cancel(MainMenuPlayer player)
    {
        base.Cancel(player);
        
        if (player.PlayerNum == 0)
        {
            if (allReadyOverlay.isActiveAndEnabled)
            {
                allReadyOverlay.SetActive(false);
            }
        }
    }

    public override void Submit(MainMenuPlayer player)
    {
        base.Submit(player);

        if (player.PlayerNum == 0)
        {
            if (allReadyOverlay.isActiveAndEnabled)
            {
                raceSettings.StartRace();
            }
        }
    }
}