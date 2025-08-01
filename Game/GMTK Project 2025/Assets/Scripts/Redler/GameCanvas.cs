using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    //For later use

    public static GameCanvas Instance;

    private bool isPaused;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private AudioVariable pauseAudio;
    [SerializeField] private AudioVariable resumeAudio;
    [SerializeField] private AudioVariable mainMenuAudio;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pausePanel.SetActive(false);

        resumeButton.onClick.AddListener(Pause);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void GoToMainMenu()
    {
        AudioManager.Instance.PlayAudio(mainMenuAudio);

        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void Pause()
    {
        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);

        if (isPaused)
        {
            AudioManager.Instance.PlayAudio(pauseAudio);
        }
        else
        {
            AudioManager.Instance.PlayAudio(resumeAudio);
        }
    }

}
