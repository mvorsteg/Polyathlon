using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Racer))]
public class PlayerUI : MonoBehaviour
{
    public Text text;
    private Racer racer;
    private RaceManager manager;
    // Start is called before the first frame update
    void Start()
    {
        racer = GetComponent<Racer>();
        manager = GameObject.FindObjectOfType<RaceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = manager.GetPosition();
    }
}
