using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class RaceManager : MonoBehaviour
{
    public static RaceManager instance;

    public Transform startingPositionsParent;
    private Transform[] startingPositions;
    private RaceSettings raceSettings;

    private List<(Racer, int, float)> positions;
    private List<(Racer, float)> finalPositions;
    private float time;

    private bool isRaceActive = false;
    private static bool canLoadMenu = true;

    private Racer[] racers;
    private int racersAlreadyFinished;
    private int realPlayersInRace;

    public CheckpointChain chain;

    public GameObject resultsText;
    public TextMeshProUGUI placeText;
    public TextMeshProUGUI timeText;
    public Image panel;
    public TextMeshProUGUI menuText;

    public GameObject dummyUI;

    public static float Time { get => instance.time; }
    public static bool IsRaceActive { get => instance.isRaceActive; }
    public GameObject raceCourseTester;
    public bool dontAddRacers = false; // use this for non-racing test scenes
    [SerializeField]
    private bool isTrainingCourse = false;
    public static bool IsTrainingCourse { get => instance.isTrainingCourse; } // use this for training scenes

    private bool testRun = false;

    private void Awake()
    {
        instance = this;
        if (!dontAddRacers)
        {
            // race starter code
            raceSettings = FindAnyObjectByType<RaceSettings>();
            if (raceSettings == null)
            {
                Debug.Log("RaceSettings not found, instantiating a race settings for testing!");
                raceSettings = Instantiate(raceCourseTester).GetComponentInChildren<RaceSettings>();
            }

            testRun = raceSettings.testSettings;

            // get all the starting positions
            startingPositions = new Transform[startingPositionsParent.childCount];
            for (int i = 0; i < startingPositionsParent.childCount; i++)
            {
                startingPositions[i] = startingPositionsParent.GetChild(i);
            }

            // Get the racers
            // First instantiate the NPCs
            List<CharacterRegistry> npcChoices = new List<CharacterRegistry>();
            if (!isTrainingCourse)
            {
                npcChoices = raceSettings.GetNPCChoices();
                for (int i = 0; i < npcChoices.Count; i++)
                {
                    Racer racer = Instantiate(npcChoices[i].npcObj, startingPositions[i].position, startingPositions[i].rotation).GetComponent<Racer>();
                    // Change movement mode of NPCs if necessary
                    if (SceneManager.GetActiveScene().name == "Course 2")
                    {
                        racer.movementMode = Movement.Mode.GetOffTheBoat;
                    }
                    racer.name = npcChoices[i].displayName;
                }
            }
            // Next instantiate the players
            if (!testRun)
            {
                List<RaceSettings.PlayerChoice> playerChoices = raceSettings.PlayerChoices;
                for (int i = 0; i < playerChoices.Count; i++)
                {
                    PlayerInput newPlayer = PlayerInput.Instantiate(playerChoices[i].character.playerObj, playerChoices[i].playerNumber,
                                                                    playerChoices[i].controlScheme.ToString(), -1, playerChoices[i].inputDevices);
                    newPlayer.transform.position = startingPositions[i + npcChoices.Count].position;
                    newPlayer.transform.rotation = startingPositions[i + npcChoices.Count].rotation;

                    PlayerController playerRacer = newPlayer.GetComponent<PlayerController>();
                    playerRacer.name = playerChoices[i].character.name + " (P" + (playerChoices[i].playerNumber + 1) + ")";
                    playerRacer.SetPlayerNumber(playerChoices[i].playerNumber);

                    newPlayer.GetComponentInChildren<UI>().SetScale(i, playerChoices.Count);

                    // remove excess audio listeners
                    if (i > 0)
                    {
                        Destroy(newPlayer.GetComponentInChildren<AudioListener>());
                    }
                }
                // activate dummy camera for 3 player splitscreen
                if (playerChoices.Count == 3)
                {
                    dummyUI.SetActive(true);
                }
                realPlayersInRace = playerChoices.Count;
            }
            else
            {
                Transform testPlayer = raceSettings.testCharacterGameObject.transform;
                testPlayer.position = startingPositions[npcChoices.Count].position;
                testPlayer.rotation = startingPositions[npcChoices.Count].rotation;
                realPlayersInRace = 1;
            }
            //Destroy(raceSettings.gameObject);
        }
    }

    private void Start() 
    {
        racers = FindObjectsByType<Racer>(FindObjectsSortMode.None);
        if (!isTrainingCourse)
        {
            foreach(Racer r in racers)
            {
                r.nextCheckpoint = chain.GetFirstCheckpoint();
            }
            positions = new List<(Racer, int, float)>(racers.Length);
            finalPositions = new List<(Racer, float)>(racers.Length);
        }
        if (resultsText != null)
            resultsText.SetActive(false);
        if (timeText != null)
            timeText.gameObject.SetActive(false);
        if (placeText != null)
            placeText.gameObject.SetActive(false);
        if (panel != null)
            panel.enabled = false;
        if (menuText != null)
            menuText.gameObject.SetActive(false);
        canLoadMenu = true;
    }

    private void Update()
    {
        if (!Application.isEditor && Input.GetKey(KeyCode.Escape))
        {
            ReturnToMenu();
        }
        // update positions of racers (1st, 2nd...)
        if (isRaceActive && !isTrainingCourse)
        {
            positions.Clear();
            foreach(Racer r in racers)
            {
                if (!r.isFinished)
                {
                    positions.Add((r, r.nextCheckpoint.seq, Vector3.SqrMagnitude(r.nextCheckpoint.transform.position - r.transform.position)));
                }
            }
            positions.Sort((a, b) => (b.Item2.CompareTo(a.Item2) == 0 ? a.Item3.CompareTo(b.Item3) : b.Item2.CompareTo(a.Item2)));

            // update time
            time += UnityEngine.Time.deltaTime;
        }
    }

    public static void RespawnPlayer(PlayerController player)
    {
        int playerNumber = player.GetPlayerNumber();
        if (playerNumber == -1)
        {
            playerNumber = 0;
        }
        player.transform.position = instance.startingPositions[playerNumber].position;
        player.Land();
        player.SetMovementMode(Movement.Mode.Running);
        // Remove velocity
        player.Revive(true);
        Rigidbody playerRb = player.transform.GetComponent<Rigidbody>();
        playerRb.isKinematic = true;
        playerRb.isKinematic = false;   
    }

    public static void StartRace()
    {
        instance.isRaceActive = true;
        foreach (Racer r in instance.racers)
        {
            r.StartRace();
        }
    }

    public static void FinishRace(Racer racer, float extraTime = 0f)
    {
        Debug.Log("racer " + racer);
        instance.racersAlreadyFinished++;
        instance.finalPositions.Add((racer, instance.time + extraTime));
        if (racer is PlayerController)
        {
            instance.realPlayersInRace--;
            if (instance.realPlayersInRace == 0)
            {
                foreach(Racer r in instance.racers)
                {
                    if (!r.isFinished && r is NPC)
                    {
                        r.FinishRace(true);
                    }
                }
                instance.StartCoroutine(instance.DisplayScores());
            }
        }
    }

    public static int GetPosition(Racer racer)
    {
        for (int i = 0; i < instance.positions.Count; i++)
        {
            if (instance.positions[i].Item1 == racer)
            {
                return i + 1 + instance.racersAlreadyFinished;
            }
        }
        return 0;
    }

    private IEnumerator DisplayScores()
    {
        finalPositions.Sort((a, b) => (a.Item2.CompareTo(b.Item2)));
        yield return new WaitForSeconds(2f);
        string names = "";
        string times = "";
        for (int i = 0; i < racers.Length; i++)
        {
            names += (i+1) + (i < 9 ? "    " : "   ") + finalPositions[i].Item1.name;
          /*
            if (racers[i] is PlayerController)
            {
                names += "(P" + (((PlayerController)racers[i]).GetPlayerNumber() + 1) + ")";
            }
            */
            names += '\n';
            times += UI.FormatTime(finalPositions[i].Item2) + '\n';
        }
        placeText.text = names;
        timeText.text = times;
        resultsText.SetActive(true);
        timeText.gameObject.SetActive(true);
        placeText.gameObject.SetActive(true);
        panel.enabled = true;

        yield return new WaitForSeconds(2f);
        
        menuText.gameObject.SetActive(true);
        if (raceSettings.HasNextRace)
        {
            menuText.text = "Press any button to continue to next race";
        }
        else
        {
            menuText.text = "Press any button to return to menu";
        }
        
        foreach (Racer r in racers)
        {
            r.RaceIsOver();
        }
    }

    public static void ReturnToMenu()
    {
        if (IsRaceActive)
        {
            instance.isRaceActive = false;
            if (instance.raceSettings.HasNextRace)
            {
                instance.raceSettings.StartNextRace();
            }
            else
            {
                instance.raceSettings.EndRace();
                Debug.Log("return to main menu please!");
                if (canLoadMenu)
                {
                    canLoadMenu = false;
                    Debug.Log("Okay loading main menu!");
                    SceneManager.LoadScene("Main Menu");
                }
            }
        }
    }

    /*  if it looks stupid but it works, it ain't stupid */
    public static Racer GetRacerOtherThanThisOne(Racer racer)
    {
        Racer chosen = racer;
        while (chosen == racer)
        {
            chosen = instance.racers[Random.Range(0, instance.racers.Length)];
        }
        return chosen;
    }
}