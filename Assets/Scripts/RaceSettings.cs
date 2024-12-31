using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RaceSettings : MonoBehaviour
{
    public class PlayerChoice
    {
        public int playerNumber;
        public CharacterRegistry character;
        public ControlScheme controlScheme;
        public InputDevice[] inputDevices;

        // constructor
        public PlayerChoice(int playerNumber, CharacterRegistry character, ControlScheme controlScheme, InputDevice[] inputDevices)
        {
            this.playerNumber = playerNumber;
            this.character = character;
            this.controlScheme = controlScheme;
            this.inputDevices = inputDevices;
        }
    }

    public GameMode mode;

    public CharacterList characterList;
    private CharacterRegistry[] characters;
    public List<CharacterRegistry> npcChoices;

    // Test options for when running the race directly from the race scene instead of from the main menu
    public bool testSettings = false;
    public int testCpuQuantity = 11;
    public GameObject testCharacterGameObject;

    public StageRegistry selectedStage;
    public List<StageRegistry> preloadedStages;
    public int numRaces;
    private int currRaceIdx = 0;
    public RaceSelection raceSelection;
    public CPUDifficulty cpuDifficulty;
    public int numCPUs;
    public string mainMenuSceneName;

    public List<PlayerChoice> PlayerChoices { get; private set; }

    public bool HasNextRace { get => currRaceIdx < numRaces - 1; }
    public bool IsMidRace { get; protected set; }
    
    public static RaceSettings instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
        PlayerChoices = new List<PlayerChoice>();
        npcChoices = new List<CharacterRegistry>();
        if (characters == null)
        {
            characters = characterList.GetCharacters();
        }
    }

    // Called by MainMenuManager
    public void AddPlayerChoice(int newPlayerNum, CharacterRegistry newCharacter, ControlScheme newControlScheme, InputDevice[] newInput)
    {
        PlayerChoices.Add(new PlayerChoice(newPlayerNum, newCharacter, newControlScheme, newInput));
    }

    // overload taking a premade PlayerChoice object
    public void AddPlayerChoice(PlayerChoice newPlayerChoice)
    {
        PlayerChoices.Add(newPlayerChoice);
    }

    public void ClearPlayerChoices()
    {
        PlayerChoices.Clear();
    }

    public List<CharacterRegistry> GetNPCChoices()
    {
        if (testSettings)
        {
            this.numCPUs = testCpuQuantity;
            AssignNPCChoices();
        }
        return npcChoices;
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
        selectedStage = preloadedStages[0];
    }

    public void SetRaceParams(int numRaces, RaceSelection raceSelection, CPUDifficulty cpuDifficulty, int numCPUs)
    {
        this.numRaces = numRaces;
        this.raceSelection = raceSelection;
        this.cpuDifficulty = cpuDifficulty;
        this.numCPUs = numCPUs;
        AssignNPCChoices();
    }

    public void StartRace()
    {
        if (IsMidRace)
        {
            currRaceIdx++;
        }
        else
        {
            currRaceIdx = 0;
            IsMidRace = true;
        }
        SceneManager.LoadScene(selectedStage.sceneName);
    }

    public void EndRace()
    {
        IsMidRace = false;
    }

    public void StartNextRace()
    {
        currRaceIdx++;
        if (raceSelection == RaceSelection.InOrder || raceSelection == RaceSelection.Random)
        {
            if (currRaceIdx < preloadedStages.Count)
            {
                selectedStage = preloadedStages[currRaceIdx];
                SceneManager.LoadScene(selectedStage.sceneName);
            }
        }
        else if (raceSelection == RaceSelection.P1Choose)
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    private void AssignNPCChoices()
    {
        if (numCPUs > 0)
        {
            npcChoices = new List<CharacterRegistry>();
            // make a copy of the characters list
            if (characters == null)
            {
                characters = characterList.GetCharacters();
            }
            List<CharacterRegistry> availableCharacters = characters.ToList();
            // remove all the characters that are taken from the list
            if (PlayerChoices != null)
            {
                foreach (PlayerChoice playerChoice in PlayerChoices)
                {
                    availableCharacters.Remove(playerChoice.character);
                }
            }
            // choose characters randomly for NPCs, avoiding repetition until we run out of characters
            for (int i = 0; i < numCPUs; i++)
            {
                if (availableCharacters.Count > 0)
                {
                    int choiceIndex = Random.Range(0, availableCharacters.Count);
                    npcChoices.Add(availableCharacters[choiceIndex]);
                    availableCharacters.RemoveAt(choiceIndex);
                }
                else
                {
                    npcChoices.Add(characters[(int)Mathf.Round(Random.Range(0, characters.Length))]);
                }
            }
        }
    }


}
