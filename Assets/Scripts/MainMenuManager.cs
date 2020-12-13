using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    public GameObject openingUI;
    private PlayerInputManager inputManager;
    private List<MainMenuPlayer> players;
    private const float distanceBetweenPlayers = 1.3f;

    void Awake()
    {
        players = new List<MainMenuPlayer>();
    }

    // Each instantiated player object will call this so that they can be managed
    public void JoinPlayer(MainMenuPlayer newPlayer)
    {
        if (openingUI.activeSelf)
            openingUI.SetActive(false);
        players.Add(newPlayer);
        newPlayer.SetPlayerNum(players.Count - 1);
        ArrangePlayers();
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


}
