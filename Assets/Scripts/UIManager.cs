using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject livesArea;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI durabilityText;


    private void Start()
    {
    }

    private void Update()
    {
        timerText.text = SceneManager.instance.currentTime.ToString();
        durabilityText.text = SceneManager.instance.currentDurability.ToString();
        UpdateLivesImages();
    }

    public void UpdateLivesImages()
    {
        for(int i = 0; i < livesArea.transform.childCount; i++)
        {
            if (SceneManager.instance.currentLives > i)
            {
                livesArea.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                livesArea.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
