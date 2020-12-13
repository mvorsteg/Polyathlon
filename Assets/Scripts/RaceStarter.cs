using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RaceStarter : MonoBehaviour
{
    public Transform startingPositionsParent;
    private Transform[] startingPositions;
    private RaceSettings raceSettings;
    

    void Awake()
    {
        try {
            raceSettings = GameObject.FindObjectsOfType<RaceSettings>()[0];
            // get all the starting positions
            startingPositions = new Transform[startingPositionsParent.childCount];
            for (int i = 0; i < startingPositionsParent.childCount; i++)
            {
                startingPositions[i] = startingPositionsParent.GetChild(i);
            }

            // Get the racers
            List<Character> npcChoices = raceSettings.GetNPCChoices();
            List<RaceSettings.PlayerChoice> playerChoices = raceSettings.GetPlayerChoices();
            // First instantiate the NPCs
            for (int i = 0; i < npcChoices.Count; i++)
            {
                Racer racer = Instantiate(npcChoices[i].npcObj, startingPositions[i].position, startingPositions[i].rotation).GetComponent<Racer>();
                // Change movement mode of NPCs if necessary
                if (SceneManager.GetActiveScene().name == "Course 2")
                {
                    racer.movementMode = Movement.Mode.GetOffTheBoat;
                }
            }
            // instantiate the players
            for (int i = 0; i < playerChoices.Count; i++)
            {
                PlayerInput newPlayer = PlayerInput.Instantiate(playerChoices[i].character.playerObj, playerChoices[i].playerIndex,
                                                                playerChoices[i].controlScheme, -1, playerChoices[i].inputDevices);
                newPlayer.transform.position = startingPositions[i + npcChoices.Count].position;
                newPlayer.transform.rotation = startingPositions[i + npcChoices.Count].rotation;
                // remove excess audio listeners
                if (i > 0)
                {
                    Destroy(newPlayer.GetComponentInChildren<AudioListener>());
                }
            }
            
        }
        catch (System.Exception)
        {
            Debug.LogWarning("Where are the RaceSettings?!?!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
