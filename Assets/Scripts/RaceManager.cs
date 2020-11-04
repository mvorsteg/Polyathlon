using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaceManager : MonoBehaviour
{
    private List<(Racer, int, float)> positions;

    public int numRacers;

    public CheckpointChain chain;

    private Racer[] racers;
    private void Start() 
    {
        racers = GameObject.FindObjectsOfType<Racer>();
        foreach(Racer r in racers)
        {
            r.nextCheckpoint = chain.GetFirstCheckpoint();
        }
        positions = new List<(Racer, int, float)>(numRacers);
    }

    private void Update()
    {
        positions.Clear();
        foreach(Racer r in racers)
        {
            positions.Add((r, r.nextCheckpoint.seq, Vector3.SqrMagnitude(r.nextCheckpoint.transform.position - r.transform.position)));
        }
        positions.Sort((a, b) => (b.Item2.CompareTo(a.Item2) == 0 ? a.Item3.CompareTo(b.Item3) : b.Item2.CompareTo(a.Item2)));    
    }

    public string GetPosition()
    {
        string str = "";
        int i = 1;
        foreach((Racer, int, float) position in positions)
        {
            str += position.Item2 + " : " + position.Item1.transform.name + " "+ position.Item3.ToString("0.00") + '\n';
            //str += i++ + " : " + position.Item1.transform.name + '\n';
        }
        return str;
    }
}