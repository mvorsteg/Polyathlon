using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MainMenuPlayer : MonoBehaviour
{
    public CharacterList characterList;
    public TextMeshPro nameText;
    public TextMeshPro playerNumText;
    public TextMeshPro readyText;
    private PlayerInput playerInput;
    private MasterMenuUI menuUI;
    private MainMenuManager manager;
    private CharacterRegistry[] characters;
    private int characterIndex;
    private GameObject currentCharPreview;
    private bool canCycle = false;
    private string unreadyMessage; // displayed when the player hasn't said they're ready
    private ControlScheme controlScheme;
    
    public int PlayerNum { get; set; }
    public ControlScheme ControlScheme { get => controlScheme; }
    public InputDevice[] InputDevices { get => playerInput.devices.ToArray(); }
    private void Awake()
    {
        
        playerInput = GetComponent<PlayerInput>();
        manager = FindAnyObjectByType<MainMenuManager>();
        menuUI = FindFirstObjectByType<MasterMenuUI>();
    }

    private void Start()
    {
        transform.position += new Vector3(0, 0.36f, 0);
        canCycle = true;
        controlScheme = ((InputControlScheme)playerInput.user.controlScheme).name == "Gamepad" ? ControlScheme.Gamepad : ControlScheme.Keyboard;
        unreadyMessage = (controlScheme == ControlScheme.Gamepad ? "Ready? Press A!" : "Ready? Press Space!");
        readyText.text = unreadyMessage;
        menuUI.AddPlayer(this, controlScheme );
    }

    public void Exit()
    {
        menuUI.RemovePlayer(this);
        Destroy(this.gameObject);
    }

    // ----------------- INPUT EVENTS --------------------
    public void OnNavigate(InputValue value)
    {
        Vector2 vecVal = value.Get<Vector2>();
        if (canCycle)
        {
            Debug.Log(string.Format("x:{0}, y:{1}", vecVal.x, vecVal.y));
            menuUI.Navigate(this, vecVal);
            StartCoroutine(PreventSpeedyJoysticks());
        }
        // if (canCycle)
        // {
        //     StartCoroutine(PreventSpeedyJoysticks());
        //     if (!ready)
        //     {
        //         if (vecVal.x == 1)
        //         {
        //             CycleCharacter(true);
        //         }
        //         else if (vecVal.x == -1)
        //         {
        //             CycleCharacter(false);
        //         }
        //     }
        //     manager.Increment(vecVal);
        // }
    }

    public void OnSubmit()
    {
        // List<MainMenuManager.MenuMode> confirmables = new List<MainMenuManager.MenuMode> { MainMenuManager.MenuMode.CPUSelect,
        //                                             MainMenuManager.MenuMode.StageSelect,
        //                                             MainMenuManager.MenuMode.ModeSelect};
        // if (canConfirm && manager.currentMenuMode == MainMenuManager.MenuMode.CharacterSelect)
        // {
        //     ready = true;
        //     readyText.text = "Ready!";
        //     manager.InformReady(true);
        // }
        // else if (confirmables.Contains(manager.currentMenuMode))
        // {
        //     manager.Confirm(true);
        // }
        menuUI.Submit(this);
    }

    public void OnConfirmSelections()
    {
        // if (manager.currentMenuMode == MainMenuManager.MenuMode.CharacterSelect)
        //     manager.Confirm(true);
        menuUI.Confirm(this);
    }

    public void OnCancel()
    {
        menuUI.Cancel(this);
        // if (manager.currentMenuMode == MainMenuManager.MenuMode.CharacterSelect)
        // {
        //     if (canConfirm && ready)
        //     {
        //         ready = false;
        //         readyText.text = unreadyMessage;
        //         manager.InformReady(false);
        //     }
        //     else if (canConfirm) // if we hadn't already readied, then delete this player
        //     {
        //         // Inform the manager that we quit
        //         manager.UnjoinPlayer(playerNum);
        //         Destroy(gameObject);
        //     }
        // }
        // else
        // {
        //     manager.Confirm(false);
        // }
    }

    public void OnAnyKey()
    {
        //Debug.Log("AnyKey");
        //menuUI.AnyKeyPressed();
    }

    // ----------------- END INPUT EVENTS --------------------

    public void SetPreviewVisibility(bool visible)
    {
        currentCharPreview.SetActive(visible);
        nameText.gameObject.SetActive(visible);
        playerNumText.gameObject.SetActive(visible);
        readyText.gameObject.SetActive(visible);
    }

    public int GetPlayerIndex()
    {
        return playerInput.playerIndex;
    }

    public RaceSettings.PlayerChoice GetPlayerChoice()
    {
        return new RaceSettings.PlayerChoice(PlayerNum, characters[characterIndex], controlScheme, playerInput.devices.ToArray());
    }

    // Joysticks on gamepads are gonna trigger CycleCharacer way too fast
    // if a buffer isn't put between each cycle
    private IEnumerator PreventSpeedyJoysticks()
    {
        canCycle = false;
        yield return new WaitForSeconds(0.18f);
        canCycle = true;
    }
}
