using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public enum MenuMode
    {
        Opening,
        CharacterSelect,
        CPUSelect,
        StageSelect,
    }

    public MenuMode currentMenuMode;
    public GameObject openingUI;
    public GameObject back;
    public TextMeshProUGUI confirmText;
    public TextMeshProUGUI bottomScreenInfo;
    public TextMeshProUGUI numCPUsText;
    public GameObject chooseStage;
    public GameObject[] stageUI;
    private int stageIndex;
    private int numCPUs = 4;
    private PlayerInputManager inputManager;
    private RaceSettings raceSettings;
    private List<MainMenuPlayer> players;
    private List<string> controlSchemes;
    private string confrimMessage;
    private bool allReady;
    private const float distanceBetweenPlayers = 1.3f;
    private const int maxRacers = 12;

    void Awake()
    {
        raceSettings = FindObjectsOfType<RaceSettings>()[0];
        players = new List<MainMenuPlayer>();
        controlSchemes = new List<string>();
    }

    // Each instantiated player object will call this so that they can be managed
    public void JoinPlayer(MainMenuPlayer newPlayer)
    {
        if (openingUI.activeSelf)
            openingUI.SetActive(false);
            back.SetActive(true);
            bottomScreenInfo.gameObject.SetActive(true);
            currentMenuMode = MenuMode.CharacterSelect;
        players.Add(newPlayer);
        controlSchemes.Add(newPlayer.GetControlScheme());
        newPlayer.SetPlayerNum(players.Count - 1);
        // if everyone was ready before, they aren't now
        InformReady(false);
        // update positions of players
        ArrangePlayers();
        UpdateMessages();
    }

    // Called when a player quits
    public void UnjoinPlayer(int playerIndex)
    {
        // remove the player from our lists
        controlSchemes.RemoveAt(playerIndex);
        players.RemoveAt(playerIndex);
        if (players.Count > 0)
        {
            // reassign player numbers
            for (int i = 0; i < players.Count; i++)
            {
                players[i].SetPlayerNum(i);
            }
            // see if everyone's ready now
            InformReady(true);
            // rearrange their positions
            ArrangePlayers();
            UpdateMessages();
        }
        else // go back to main title if everyone quit
        {
            openingUI.SetActive(true);
            back.SetActive(false);
        }
    }

    // Determines if we're using keyboard, gamepads, or both
    private void UpdateMessages()
    {
        if (controlSchemes.Contains("Gamepad") && controlSchemes.Contains("Keyboard"))
        {
            confrimMessage = "Press Enter or Start to Confirm!";
        }
        else if (controlSchemes.Contains("Gamepad"))
        {
            confrimMessage = "Press Start to Confirm!";
        }
        else
        {
            confrimMessage = "Press Enter to Confirm!";
        }
        switch (players.Count)
        {
            case 1:
                bottomScreenInfo.text = "You can join up to 3 more players! (Press A on Gamepads)";
            break;
            case 2:
                bottomScreenInfo.text = "You can join up to 2 more players! (Press A on Gamepads)";
            break;
            case 3:
                bottomScreenInfo.text = "You can join 1 more player! (Press A on Gamepads)";
            break;
            case 4:
                bottomScreenInfo.text = "";
            break;
            default:
            break;
        }
    }

    // Postitions the players based on how many there are
    private void ArrangePlayers()
    {
        float totalDist = (players.Count - 1) * distanceBetweenPlayers;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = new Vector3(distanceBetweenPlayers * i - totalDist / 2, players[i].transform.position.y, players[i].transform.position.z);
        }
    }

    // MainMenuPlayer calls this when they are set to ready or unready
    public void InformReady(bool ready)
    {
        if (ready)
        {
            allReady = true;
            // check if every player is ready
            foreach(MainMenuPlayer player in players)
            {
                if (!player.IsReady())
                {
                    allReady = false;
                }
            }
        }
        else
        {
            allReady = false;
        }
        // ready to go
        if (allReady)
        {
            confirmText.gameObject.SetActive(true);
            confirmText.text = confrimMessage;
        }
        else // not ready to go
        {
            confirmText.gameObject.SetActive(false);
        }
    }

    // Called by MainMenuPlayers
    // Handles all transitions after the character select screen forwards and backwards
    public void Confirm(bool confirm)
    {
        switch(currentMenuMode)
        {
            case MenuMode.CharacterSelect:
                if (allReady && confirm)
                {
                    foreach(MainMenuPlayer player in players)
                    {
                        player.SetPreviewVisibility(false);
                    }
                    numCPUsText.gameObject.SetActive(true);
                    numCPUsText.text = "Number of CPUs: <[" + numCPUs + "]>";
                    confirmText.gameObject.SetActive(false);
                    bottomScreenInfo.text = "Press Space on Keyboard or A on Gamepad to confirm!";
                    currentMenuMode = MenuMode.CPUSelect;
                }
            break;
            case MenuMode.CPUSelect:
                if (confirm)
                {
                    numCPUsText.gameObject.SetActive(false);
                    chooseStage.SetActive(true);
                    stageUI[stageIndex].SetActive(true);
                    currentMenuMode = MenuMode.StageSelect;
                }
                else
                {
                    foreach(MainMenuPlayer player in players)
                    {
                        player.SetPreviewVisibility(true);
                    }
                    currentMenuMode = MenuMode.CharacterSelect;
                    numCPUsText.gameObject.SetActive(false);
                    confirmText.gameObject.SetActive(false);
                    UpdateMessages();
                }
            break;
            case MenuMode.StageSelect:
                if (confirm) // This is where we actually switch to the race scene
                {
                    foreach (MainMenuPlayer player in players)
                    {
                        raceSettings.AddPlayerChoice(player.GetPlayerChoice());
                        raceSettings.EnterRace(numCPUs, stageIndex);
                    }
                }
                else
                {
                    chooseStage.SetActive(false);
                    stageUI[stageIndex].SetActive(false);
                    currentMenuMode = MenuMode.CPUSelect;
                }
            break;
            default:
            break;
        }
    }

    // Called by MainMenuPlayers to select things like the number of CPUs and the stage
    public void Increment(Vector2 value)
    {
        switch (currentMenuMode)
        {
            case MenuMode.CPUSelect:
                if (value.x == 1 && numCPUs + 1 + players.Count <= maxRacers)
                {
                    numCPUs++;
                }
                else if (value.x == -1 && numCPUs - 1 >= 0)
                {
                    numCPUs--;
                }
                numCPUsText.text = "Number of CPUs: <[" + numCPUs + "]>";
            break;
            case MenuMode.StageSelect:
                if (value.x == 1)
                {
                    stageUI[stageIndex].SetActive(false);
                    stageIndex = (stageIndex + 1) % stageUI.Length;
                }
                else if (value.x == -1)
                {
                    stageUI[stageIndex].SetActive(false);
                    if (stageIndex - 1 >= 0)
                        stageIndex--;
                    else
                        stageIndex = stageUI.Length - 1;
                }
                stageUI[stageIndex].SetActive(true);
                
            break;
            default:
            break;
        }
        
    }


}
