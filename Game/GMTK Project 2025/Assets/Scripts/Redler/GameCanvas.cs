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

    [SerializeField] private AudioClip pauseClip;
    [SerializeField] private float pauseAudioVolume = 1f;

    [SerializeField] private AudioClip resumeClip;
    [SerializeField] private float resumeAudioVolume = 1f;

    [SerializeField] private AudioClip mainMenuClip;
    [SerializeField] private float mainMenuAudioVolume = 1f;

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
        AudioManager.Instance.PlayAudio(mainMenuClip, mainMenuAudioVolume);

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
            AudioManager.Instance.PlayAudio(pauseClip, pauseAudioVolume);
        }
        else
        {
            AudioManager.Instance.PlayAudio(resumeClip, resumeAudioVolume);
        }
    }

}
