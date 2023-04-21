using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    public int maxDurability;
    public float timeLimit;
    public int maxLives;
    [HideInInspector] public float currentTime;
    [HideInInspector] public int currentDurability;
    [HideInInspector] public int currentLives;

    [HideInInspector] public float sceneTime;
    [HideInInspector] public int sceneDurability;
    [HideInInspector] public int sceneLives;

    [HideInInspector] public bool sceneOver;

    public Vector2 maxSceneBounds;
    public Vector2 minSceneBounds;
    public Vector2 ballStartPosition;
    private float lastTime;
    private BombController bomb;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        sceneDurability = 0;
        sceneLives = 0;
        sceneTime = 0;
        currentTime = timeLimit;
        currentDurability = maxDurability;
        currentLives = GameManager.instance.currentLives;
        lastTime = Time.time;
        bomb = GameObject.FindGameObjectWithTag("Player").GetComponent<BombController>();
        sceneOver = false;
    }

    public void DecrementClock()
    {
        float elapsedTime = Time.time - lastTime;
        if (elapsedTime > 1f)
        {
            currentTime--;
            sceneTime++;
            lastTime = Time.time;
        }
        if(currentTime <= 0) // Blow up ball
        {
            bomb.BlowUp();
        }
    }

    public void LoseLife()
    {
        GameManager.instance.currentLives--;
        currentLives = GameManager.instance.currentLives;
        sceneLives++;
        if(currentLives == 0)
        {
            // End the game
        }
        else
        {
            currentDurability = maxDurability;
            currentTime = timeLimit;
            bomb.transform.position = ballStartPosition;
        }
    }

}
