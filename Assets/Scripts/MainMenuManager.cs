using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    public GameObject pressAnyButton;
    private PlayerInputManager inputManager;
    private List<MainMenuPlayer> players;

    void Awake()
    {
        players = new List<MainMenuPlayer>();
    }


    public void JoinPlayer(MainMenuPlayer newPlayer)
    {
        if (pressAnyButton.activeSelf)
            pressAnyButton.SetActive(false);
        players.Add(newPlayer);
    }


}
