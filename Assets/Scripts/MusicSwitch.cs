using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MusicSwitch : MonoBehaviour
{
    public int musicIndex = 0;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GetComponentInParent<AudioManager>();    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.transform.GetComponent<PlayerController>() != null && audioManager.MusicPlaying != musicIndex)
        {
            StartCoroutine(audioManager.SwitchSong(musicIndex));
        }
    }
}