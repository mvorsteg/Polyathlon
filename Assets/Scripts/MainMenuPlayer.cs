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
    private int playerNum; // player num is indexed to 0
    private MasterMenuUI menuUI;
    private MainMenuManager manager;
    private Character[] characters;
    private int characterIndex;
    private GameObject currentCharPreview;
    private bool canCycle = false;
    private bool canConfirm = false;
    private bool ready;
    private string unreadyMessage; // displayed when the player hasn't said they're ready
    private ControlScheme controlScheme;
    
    public int PlayerNum { get => playerNum; }
    public ControlScheme ControlScheme { get => controlScheme; }
    private void Awake()
    {
        
        playerInput = GetComponent<PlayerInput>();
        manager = GameObject.FindObjectsOfType<MainMenuManager>()[0];
        menuUI = FindFirstObjectByType<MasterMenuUI>();
    }

    void Start()
    {
        transform.position += new Vector3(0, 0.36f, 0);
        characters = characterList.GetCharacters();
        canCycle = true;
        controlScheme = ((InputControlScheme)playerInput.user.controlScheme).name == "Gamepad" ? ControlScheme.Gamepad : ControlScheme.Keyboard;
        unreadyMessage = (controlScheme == ControlScheme.Gamepad ? "Ready? Press A!" : "Ready? Press Space!");
        readyText.text = unreadyMessage;
        canConfirm = true;
        SelectCharacter();
        manager.JoinPlayer(this);
        menuUI.AddPlayer(this, controlScheme );
    }


    // ----------------- INPUT EVENTS --------------------
    public void OnNavigate(InputValue value)
    {
        Vector2 vecVal = value.Get<Vector2>();
        menuUI.Navigate(this, vecVal);
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

    public void OnQualityCycle()
    {
        manager.CycleQuality();
    }

    public void OnAnyKey()
    {
        //Debug.Log("AnyKey");
        //menuUI.AnyKeyPressed();
    }

    // ----------------- END INPUT EVENTS --------------------

    // assign a player number to this player, called by MainMenuManager
    public void SetPlayerNum(int num)
    {
        playerNum = num;
        playerNumText.text = "Player " + (playerNum + 1);
    }

    public void SetPreviewVisibility(bool visible)
    {
        currentCharPreview.SetActive(visible);
        nameText.gameObject.SetActive(visible);
        playerNumText.gameObject.SetActive(visible);
        readyText.gameObject.SetActive(visible);
    }

    public int GetPlayerNum()
    {
        return playerNum;
    }

    public int GetPlayerIndex()
    {
        return playerInput.playerIndex;
    }

    public bool IsReady()
    {
        return ready;
    }

    public string GetControlSchemeStr()
    {
        return controlScheme.ToString();
    }

    public Character GetCharacter()
    {
        return characters[characterIndex];
    }

    public RaceSettings.PlayerChoice GetPlayerChoice()
    {
        return new RaceSettings.PlayerChoice(playerNum, characters[characterIndex],
                    playerInput.playerIndex, controlScheme.ToString(), playerInput.devices.ToArray());
    }

    // Joysticks on gamepads are gonna trigger CycleCharacer way too fast
    // if a buffer isn't put between each cycle
    private IEnumerator PreventSpeedyJoysticks()
    {
        canCycle = false;
        yield return new WaitForSeconds(0.18f);
        canCycle = true;
    }

    // Cycle through the characters we have
    private void CycleCharacter(bool forward)
    {
        // Determine how to move our index
        if (forward)
        {
            characterIndex = (characterIndex + 1) % (characters.Length);
        }
        else
        {
            if (characterIndex - 1 >= 0)
                characterIndex--;
            else
                characterIndex = characters.Length - 1;
        }
        // remove the old character preview and instantiate a new one
        Destroy(currentCharPreview);
        SelectCharacter();
    }

    // Selects the character based on the current characterIndex and updates the name
    private void SelectCharacter()
    {
        currentCharPreview = Instantiate(characters[characterIndex].previewObj, transform.position, Quaternion.Euler(0, 180, 0), this.transform);
        nameText.text = characters[characterIndex].name;
    }
}
