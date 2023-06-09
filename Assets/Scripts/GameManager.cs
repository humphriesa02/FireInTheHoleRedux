using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used for data needed throughout the game, not scene specific
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Overall game stats stored here
    public int totalHits;
    public float totalTimeSpentInGame;
    public int totalLivesLost;
    public string currentScene;
    public int currentLives = 3;
    public AudioSource soundEffectSource;
    public SoundEffectManager soundEffects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void loseGame()
    {

    }
}
