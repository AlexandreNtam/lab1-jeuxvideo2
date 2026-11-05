using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;

    [SerializeField] private AudioClip exitReached;
    [SerializeField] private AudioClip gemCollected;
    [SerializeField] private AudioClip playerJump;
    [SerializeField] private AudioClip playerKilled;

    public static SoundManager Instance { get { return instance; } }
    public AudioClip ExitReached { get { return exitReached; } }
    public AudioClip GemCollected { get { return gemCollected; } }
    public AudioClip PlayerJump { get { return playerJump; } }
    public AudioClip PlayerKilled { get { return playerKilled; } }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }
}