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
    private MainMenuManager manager;
    private Character[] characters;
    private int characterIndex;
    private GameObject currentCharPreview;
    private bool canCycle = false;
    private bool ready;
    private string unreadyMessage; // displayed when the player hasn't said they're ready
    

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        manager = GameObject.FindObjectsOfType<MainMenuManager>()[0];
        characters = characterList.GetCharacters();
        SelectCharacter();
        manager.JoinPlayer(this);
        canCycle = true;
        Debug.Log(playerInput.user.controlScheme.name);
        //unreadyMessage = playerInput.controlScheme Gamepad ? 
    }


    // ----------------- INPUT EVENTS --------------------
    public void OnNavigate(InputValue value)
    {
        Vector2 vecVal = value.Get<Vector2>();
        if (vecVal.x == 1)
        {
            CycleCharacter(true);
        }
        else if (vecVal.x == -1)
        {
            CycleCharacter(false);
        }
    }

    public void OnSubmit()
    {
        ready = true;
        readyText.text = "Ready!";
    }

    public void OnCancel()
    {
        
    }

    // ----------------- END INPUT EVENTS --------------------

    // assign a player number to this player, called by MainMenuManager
    public void SetPlayerNum(int num)
    {
        playerNum = num;
        playerNumText.text = "Player " + (playerNum + 1);
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
        if (canCycle)
        {
            StartCoroutine(PreventSpeedyJoysticks());
            // Determine how to move our index
            if (forward)
            {
                if (characterIndex + 1 < characters.Length)
                    characterIndex++;
                else
                    characterIndex = 0;
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
    }

    // Selects the character based on the current characterIndex and updates the name
    private void SelectCharacter()
    {
        currentCharPreview = Instantiate(characters[characterIndex].previewObj, transform.position, Quaternion.Euler(0, 180, 0), this.transform);
        nameText.text = characters[characterIndex].name;
    }
}
