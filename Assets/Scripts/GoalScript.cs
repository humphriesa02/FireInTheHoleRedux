using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class GoalScript : MonoBehaviour
{
    public string nextLevel;
    public GameObject winScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.instance.sceneOver = true;
            StartCoroutine(waitToLoadNextScene());
        }
    }

    private IEnumerator waitToLoadNextScene()
    {
        winScreen.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        StopCoroutine(waitToLoadNextScene());
        UnitySceneManager.LoadScene(nextLevel);

    }
}
