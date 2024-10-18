using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RaceSettings : MonoBehaviour
{
    public class PlayerChoice
    {
        public int playerNumber;
        public Character character;
        public int playerIndex;
        public string controlScheme;
        public InputDevice[] inputDevices;

        // constructor
        public PlayerChoice(int playerNumber, Character character, int playerIndex, string controlScheme, InputDevice[] inputDevices)
        {
            this.playerNumber = playerNumber;
            this.character = character;
            this.playerIndex = playerIndex;
            this.controlScheme = controlScheme;
            this.inputDevices = inputDevices;
        }
    }

    public CharacterList characterList;
    private Character[] characters;
    private int numCPUs;
    private List<PlayerChoice> playerChoices;
    public List<Character> npcChoices;

    // Test options for when running the race directly from the race scene instead of from the main menu
    public bool testSettings = false;
    public int testCpuQuantity = 11;
    public GameObject testCharacterGameObject;
    
    void Awake()
    {
        DontDestroyOnLoad(this);
        playerChoices = new List<PlayerChoice>();
        npcChoices = new List<Character>();
        if (characters == null)
        {
            characters = characterList.GetCharacters();
        }
    }

    // Called by MainMenuManager
    public void AddPlayerChoice(int newPlayerNum, Character newCharacter, int newPlayerIndex, string newControlScheme, InputDevice[] newInput)
    {
        playerChoices.Add(new PlayerChoice(newPlayerNum, newCharacter, newPlayerIndex, newControlScheme, newInput));
    }

    // overload taking a premade PlayerChoice object
    public void AddPlayerChoice(PlayerChoice newPlayerChoice)
    {
        playerChoices.Add(newPlayerChoice);
    }

    public List<PlayerChoice> GetPlayerChoices()
    {
        return playerChoices;
    }

    public List<Character> GetNPCChoices()
    {
        if (testSettings)
        {
            this.numCPUs = testCpuQuantity;
            AssignNPCChoices();
        }
        return npcChoices;
    }

    public void EnterRace(int numCPUs, int courseId)
    {
        this.numCPUs = numCPUs;
        AssignNPCChoices();
        // load either Course 1 or Course 2
        SceneManager.LoadScene("Course " + (courseId + 1));
    }

    private void AssignNPCChoices()
    {
        if (numCPUs > 0)
        {
            npcChoices = new List<Character>();
            // make a copy of the characters list
            if (characters == null)
            {
                characters = characterList.GetCharacters();
            }
            List<Character> availableCharacters = characters.ToList();
            // remove all the characters that are taken from the list
            if (playerChoices != null)
            {
                foreach (PlayerChoice playerChoice in playerChoices)
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
