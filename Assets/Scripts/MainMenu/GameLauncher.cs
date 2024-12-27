using System.Collections.Generic;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    public List<CharacterRegistry> selectedCharacters;
    public StageRegistry selectedStage;
    public List<StageRegistry> preloadedStages;
    public int numRaces;
    public RaceSelection raceSelection;
    public CPUDifficulty cpuDifficulty;
    public int numCPUs;

    private void Awake()
    {
        selectedCharacters = new List<CharacterRegistry>();
        preloadedStages = new List<StageRegistry>();
    }

    public void SetSelectedCharacters(List<CharacterRegistry> selectedCharacters)
    {
        this.selectedCharacters.Clear();
        foreach (CharacterRegistry cr in selectedCharacters)
        {
            this.selectedCharacters.Add(cr);
        }
    }

    public void SetSelectedStage(StageRegistry selectedStage)
    {
        this.selectedStage = selectedStage;
    }
    
    public void PreloadStages(List<StageRegistry> preloadedStages)
    {
        this.preloadedStages.Clear();
        foreach (StageRegistry registry in preloadedStages)
        {
            this.preloadedStages.Add(registry);
        }
    }

    public void SetRaceSettings(int numRaces, RaceSelection raceSelection, CPUDifficulty cpuDifficulty, int numCPUs)
    {
        this.numRaces = numRaces;
        this.raceSelection = raceSelection;
        this.cpuDifficulty = cpuDifficulty;
        this.numCPUs = numCPUs;
    }

    public void StartRace()
    {
        
    }

}