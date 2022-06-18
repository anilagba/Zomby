using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    [SerializeField] Button restartButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Button exitButton;

    private void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        resumeButton.onClick.AddListener(ResumeGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    void ResumeGame()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

    void ExitGame()
    {
        Application.Quit();
    }
}
