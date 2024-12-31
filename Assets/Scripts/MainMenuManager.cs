using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class MainMenuManager : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private RaceSettings raceSettings;
    private List<MainMenuPlayer> players;
    private Vector3 titleCameraRot, charSelectCameraRot;

    private int currentQualityLevel;
    private string[] qualityLevels = {"Very Low", "Low", "Medium", "High", "Very High", "Ultra"};

    void Awake()
    {
        raceSettings = FindAnyObjectByType<RaceSettings>();
        inputManager = GetComponent<PlayerInputManager>();
        players = new List<MainMenuPlayer>();
        currentQualityLevel = QualitySettings.GetQualityLevel();
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

    }

    // Called when a player quits
    public void UnjoinPlayer(int playerIndex)
    {
        // remove the player from our lists
        players.RemoveAt(playerIndex);
    }

    // Cycles through the quality levels, called by MainMenuPlayers
    // public void CycleQuality()
    // {
    //     if (qualityLevelText.gameObject.activeSelf)
    //     {
    //         currentQualityLevel = (currentQualityLevel + 1) % qualityLevels.Length;
    //         qualityLevelText.text = "Quality: " + qualityLevels[currentQualityLevel];
    //         QualitySettings.SetQualityLevel(currentQualityLevel);
    //     }
    // }


    // Called by MainMenuPlayers to select things like the number of CPUs and the stage
    public void Increment(Vector2 value)
    {   
        // switch (currentMenuMode)
        // {
        //     case MenuMode.CPUSelect:
        //         if (value.x == 1 && numCPUs + 1 + players.Count <= maxRacers)
        //         {
        //             numCPUs++;
        //         }
        //         else if (value.x == -1 && numCPUs - 1 >= 0)
        //         {
        //             numCPUs--;
        //         }
        //         numCPUsText.text = "Number of CPUs: <[" + numCPUs + "]>";
        //     break;
        //     case MenuMode.ModeSelect:
        //         gameMode = (GameMode)(Mathf.Abs(((int)gameMode + value.x) % Enum.GetNames(typeof(GameMode)).Length));
        //         gameModeText.text = "Choose a Game Mode:\n< " + Enum.GetName(typeof(GameMode), gameMode) + " >";
        //     break;
        //     case MenuMode.StageSelect:
        //         GameObject[] stageUI;
        //         if (gameMode == GameMode.Racing)
        //         {
        //             stageUI = raceCoursesUI;
        //         }
        //         else //if (gameMode == GameMode.Training) // uncomment if adding a third mode
        //         {
        //             stageUI = trainingCoursesUI;
        //         }
        //         if (value.x == 1)
        //         {
        //             stageUI[stageIndex].SetActive(false);
        //             stageIndex = (stageIndex + 1) % stageUI.Length;
        //         }
        //         else if (value.x == -1)
        //         {
        //             stageUI[stageIndex].SetActive(false);
        //             if (stageIndex - 1 >= 0)
        //                 stageIndex--;
        //             else
        //                 stageIndex = stageUI.Length - 1;
        //         }
        //         stageUI[stageIndex].SetActive(true);
                
        //     break;
        //     default:
        //     break;
        // }
        
    }
}
