using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;


public class Menu : MonoBehaviour
{
    public void NewGame()
    {
        UnitySceneManager.LoadScene("Level1");
    }

    public void LevelSelect(string levelName)
    {
        UnitySceneManager.LoadScene(levelName);
    }
}
