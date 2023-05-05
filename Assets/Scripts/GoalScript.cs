using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class GoalScript : MonoBehaviour
{
    public string nextLevel;
    public GameObject winScreen;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.instance.sceneOver = true;
            collision.gameObject.SetActive(false);
            SceneManager.instance.currentTime = 0;
            GameManager.instance.soundEffectSource.PlayOneShot(GameManager.instance.soundEffects.bombInHole);
            StartCoroutine(waitToLoadNextScene());
        }
    }

    private IEnumerator waitToLoadNextScene()
    {
        // Wait for bomb to explode in hole
        yield return new WaitForSeconds(0.5f);
        // Bomb explodes
        anim.Play("GoalExplosion");
        GameManager.instance.soundEffectSource.PlayOneShot(GameManager.instance.soundEffects.explosion);
        IEnumerator shake = Camera.main.GetComponent<CameraController>().Shake();
        StartCoroutine(shake);
        // Wait for explosion animation
        yield return new WaitForSeconds(1f);
        // Goal post is ruined
        anim.Play("GoalRuined");
        // Show that off for a little
        yield return new WaitForSeconds(0.2f);
        // Win screen
        winScreen.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        StopCoroutine(waitToLoadNextScene());
        UnitySceneManager.LoadScene(nextLevel);

    }
}
