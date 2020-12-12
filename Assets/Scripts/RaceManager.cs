using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaceManager : MonoBehaviour
{
    public static RaceManager instance;

    private List<(Racer, int, float)> positions;
    private float time;

    private bool isRaceActive = false;

    private Racer[] racers;

    public CheckpointChain chain;

    public static float Time { get => instance.time; }
    public static bool IsRaceActive { get => instance.isRaceActive; }

    private void Awake()
    {
        instance = this;    
    }

    private void Start() 
    {
        racers = GameObject.FindObjectsOfType<Racer>();
        foreach(Racer r in racers)
        {
            r.nextCheckpoint = chain.GetFirstCheckpoint();
        }
        positions = new List<(Racer, int, float)>(racers.Length);
    }

    private void Update()
    {
        // update positions of racers (1st, 2nd...)
        if (isRaceActive)
        {
            positions.Clear();
            foreach(Racer r in racers)
            {
                positions.Add((r, r.nextCheckpoint.seq, Vector3.SqrMagnitude(r.nextCheckpoint.transform.position - r.transform.position)));
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

    public static void FinishRace(Racer racer)
    {

    }

    public static string GetPosition()
    {
        string str = "";
        //int i = 1;
        foreach((Racer, int, float) position in instance.positions)
        {
            str += position.Item2 + " : " + position.Item1.transform.name + " "+ position.Item3.ToString("0.00") + '\n';
            //str += i++ + " : " + position.Item1.transform.name + '\n';
        }
        return str;
    }
}