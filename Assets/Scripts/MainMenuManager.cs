using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class MainMenuManager : MonoBehaviour
{
    public enum MenuMode
    {
        Opening,
        CharacterSelect,
        CPUSelect,
        ModeSelect,
        StageSelect,
    }

    public enum GameMode
    {
        Racing,
        Training
    }

    public MenuMode currentMenuMode;
    public GameObject openingUI;
    public GameObject mainCamera;
    public GameObject back;
    public TextMeshProUGUI confirmText;
    public TextMeshProUGUI bottomScreenInfo;
    public TextMeshProUGUI gameModeText;
    public TextMeshProUGUI numCPUsText;
    public TextMeshProUGUI qualityLevelText;
    public GameObject chooseStage;
    public GameObject chooseTrainingCourse;
    public GameObject[] raceCoursesUI;
    public GameObject[] trainingCoursesUI;
    private GameMode gameMode = GameMode.Racing;
    private int stageIndex;
    private int numCPUs;
    private PlayerInputManager inputManager;
    private RaceSettings raceSettings;
    private List<MainMenuPlayer> players;
    private List<string> controlSchemes;
    private string confrimMessage;
    private bool allReady;
    private const float distanceBetweenPlayers = 1.3f;
    private const int maxRacers = 12;

    private bool isCameraSpinning = false;
    private Vector3 titleCameraRot, charSelectCameraRot;

    private int currentQualityLevel;
    private string[] qualityLevels = {"Very Low", "Low", "Medium", "High", "Very High", "Ultra"};

    void Awake()
    {
        raceSettings = FindObjectsOfType<RaceSettings>()[0];
        inputManager = GetComponent<PlayerInputManager>();
        players = new List<MainMenuPlayer>();
        controlSchemes = new List<string>();
        currentQualityLevel = QualitySettings.GetQualityLevel();
        qualityLevelText.text = "Quality: " + qualityLevels[currentQualityLevel];
        titleCameraRot = Camera.main.transform.localEulerAngles;
        charSelectCameraRot = new Vector3(titleCameraRot.x, titleCameraRot.y + 180f, titleCameraRot.z);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Each instantiated player object will call this so that they can be managed
    public void JoinPlayer(MainMenuPlayer newPlayer)
    {
        // If we're on the opening screen (joining the first player, transition to character select)
        if (openingUI.activeSelf)
        {
            openingUI.SetActive(false);
            back.SetActive(true);
            bottomScreenInfo.gameObject.SetActive(true);
            qualityLevelText.gameObject.SetActive(true);
            currentMenuMode = MenuMode.CharacterSelect;
            StartCoroutine(CameraSpinAround());
        }
        players.Add(newPlayer);
        Debug.Log("player index: " + newPlayer.GetPlayerIndex());
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
            qualityLevelText.gameObject.SetActive(false);
            currentMenuMode = MenuMode.Opening;
            StartCoroutine(CameraSpinAround());
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
        Debug.Log("confirm: " + confirm + " currentMenuMode: " + currentMenuMode);
        switch(currentMenuMode)
        {
            case MenuMode.CharacterSelect:
                if (allReady && confirm)
                {
                    // clear current UI
                    foreach(MainMenuPlayer player in players)
                    {
                        player.SetPreviewVisibility(false);
                    }
                    inputManager.DisableJoining();
                    confirmText.gameObject.SetActive(false);
                    // set new UI
                    gameModeText.gameObject.SetActive(true);
                    gameModeText.text = "Choose a Game Mode:\n< " + Enum.GetName(typeof(GameMode), gameMode) + " >";
                    bottomScreenInfo.text = "Press Space on Keyboard or A on Gamepad to confirm!";
                    currentMenuMode = MenuMode.ModeSelect;
                }
            break;
            case MenuMode.ModeSelect:
                if (confirm)
                {
                    // clear current UI
                    gameModeText.gameObject.SetActive(false);
                    // set new UI
                    if (gameMode == GameMode.Racing)
                    {
                        numCPUsText.gameObject.SetActive(true);
                        numCPUs = maxRacers - players.Count;
                        numCPUsText.text = "Number of CPUs: <[" + numCPUs + "]>";
                        bottomScreenInfo.text = "Press Space on Keyboard or A on Gamepad to confirm!";
                        currentMenuMode = MenuMode.CPUSelect;
                    }
                    else if (gameMode == GameMode.Training)
                    {
                        chooseTrainingCourse.SetActive(true);
                        trainingCoursesUI[stageIndex % trainingCoursesUI.Length].SetActive(true);
                        currentMenuMode = MenuMode.StageSelect;
                    }
                }
                else
                {
                    // clear current UI
                    gameModeText.gameObject.SetActive(false);
                    // set new UI
                    foreach(MainMenuPlayer player in players)
                    {
                        player.SetPreviewVisibility(true);
                    }
                    currentMenuMode = MenuMode.CharacterSelect;
                    confirmText.gameObject.SetActive(true);
                    UpdateMessages();
                    inputManager.EnableJoining();
                }
            break;
            case MenuMode.CPUSelect:
                if (confirm)
                {
                    // clear current UI
                    numCPUsText.gameObject.SetActive(false);
                    // set new UI
                    chooseStage.SetActive(true);
                    raceCoursesUI[stageIndex % raceCoursesUI.Length].SetActive(true);
                    currentMenuMode = MenuMode.StageSelect;
                }
                else
                {
                    // clear current UI
                    numCPUsText.gameObject.SetActive(false);
                    // set new UI
                    gameModeText.gameObject.SetActive(true);
                    currentMenuMode = MenuMode.ModeSelect;
                }
            break;
            case MenuMode.StageSelect:
                if (confirm) // This is where we actually switch to the race scene
                {
                    foreach (MainMenuPlayer player in players)
                    {
                        raceSettings.AddPlayerChoice(player.GetPlayerChoice());
                        if (gameMode == GameMode.Racing)
                        {
                            raceSettings.EnterRace(numCPUs, stageIndex);
                        }
                        else if (gameMode == GameMode.Training)
                        {
                            raceSettings.EnterTraining(stageIndex);
                        }
                    }
                }
                else
                {
                    if (gameMode == GameMode.Racing)
                    {
                        // clear current UI
                        chooseStage.SetActive(false);
                        // set new UI
                        raceCoursesUI[stageIndex % raceCoursesUI.Length].SetActive(false);
                        numCPUsText.gameObject.SetActive(true);
                        currentMenuMode = MenuMode.CPUSelect;
                    }
                    else if (gameMode == GameMode.Training)
                    {
                        // clear current UI
                        chooseTrainingCourse.SetActive(false);
                        // set new UI
                        trainingCoursesUI[stageIndex % trainingCoursesUI.Length].SetActive(false);
                        gameModeText.gameObject.SetActive(true);
                        currentMenuMode = MenuMode.ModeSelect;
                    }
                }
            break;
            default:
            break;
        }
    }

    // Cycles through the quality levels, called by MainMenuPlayers
    public void CycleQuality()
    {
        if (qualityLevelText.gameObject.activeSelf)
        {
            currentQualityLevel = (currentQualityLevel + 1) % qualityLevels.Length;
            qualityLevelText.text = "Quality: " + qualityLevels[currentQualityLevel];
            QualitySettings.SetQualityLevel(currentQualityLevel);
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
            case MenuMode.ModeSelect:
                gameMode = (GameMode)(Mathf.Abs(((int)gameMode + value.x) % Enum.GetNames(typeof(GameMode)).Length));
                gameModeText.text = "Choose a Game Mode:\n< " + Enum.GetName(typeof(GameMode), gameMode) + " >";
            break;
            case MenuMode.StageSelect:
                GameObject[] stageUI;
                if (gameMode == GameMode.Racing)
                {
                    stageUI = raceCoursesUI;
                }
                else //if (gameMode == GameMode.Training) // uncomment if adding a third mode
                {
                    stageUI = trainingCoursesUI;
                }
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

    private IEnumerator CameraSpinAround()
    {
        Vector3 startRot = mainCamera.transform.localEulerAngles;
        Vector3 endRot = currentMenuMode == MenuMode.CharacterSelect ? charSelectCameraRot : titleCameraRot;

        Debug.Log("Starting at " + startRot.y + " ending at " + endRot.y);

        float elapsedTime = 0f;
        float duration = 1f;

        isCameraSpinning = true;
        while (elapsedTime < duration)
        {
            mainCamera.transform.localEulerAngles = Vector3.Lerp(startRot, endRot, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.localEulerAngles = endRot;

        isCameraSpinning = false;
    }

}
