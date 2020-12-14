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
    public GameObject menuText;

    public GameObject dummyUI;

    public static float Time { get => instance.time; }
    public static bool IsRaceActive { get => instance.isRaceActive; }

    private void Awake()
    {
        instance = this;    

        // race starter code
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
                racer.name = npcChoices[i].name;
            }
            // instantiate the players
            for (int i = 0; i < playerChoices.Count; i++)
            {
                PlayerInput newPlayer = PlayerInput.Instantiate(playerChoices[i].character.playerObj, playerChoices[i].playerIndex,
                                                                playerChoices[i].controlScheme, -1, playerChoices[i].inputDevices);
                newPlayer.transform.position = startingPositions[i + npcChoices.Count].position;
                newPlayer.transform.rotation = startingPositions[i + npcChoices.Count].rotation;

                newPlayer.GetComponent<Racer>().name = playerChoices[i].character.name;

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
        catch (System.Exception)
        {
            Debug.LogWarning("Where are the RaceSettings?!?!");
        }
        Destroy(raceSettings.gameObject);
    }

    private void Start() 
    {
        racers = GameObject.FindObjectsOfType<Racer>();
        foreach(Racer r in racers)
        {
            r.nextCheckpoint = chain.GetFirstCheckpoint();
        }
        positions = new List<(Racer, int, float)>(racers.Length);
        finalPositions = new List<(Racer, float)>(racers.Length);
        resultsText.SetActive(false);
        timeText.gameObject.SetActive(false);
        placeText.gameObject.SetActive(false);
        menuText.SetActive(false);
        canLoadMenu = true;
    }

    private void Update()
    {
        // update positions of racers (1st, 2nd...)
        if (isRaceActive)
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
            names += (i+1) + (i < 9 ? "    " : "   ") + finalPositions[i].Item1.name + '\n';
            times += UI.FormatTime(finalPositions[i].Item2) + '\n';
        }
        placeText.text = names;
        timeText.text = times;
        resultsText.SetActive(true);
        timeText.gameObject.SetActive(true);
        placeText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        menuText.SetActive(true);
        foreach (Racer r in racers)
        {
            r.RaceIsOver();
        }
    }

    public static void ReturnToMenu()
    {
        if (canLoadMenu)
        {
            canLoadMenu = false;
            SceneManager.LoadScene("Main Menu");
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