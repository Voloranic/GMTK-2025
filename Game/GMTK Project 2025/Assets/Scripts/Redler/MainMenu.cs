using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private AudioClip playClip;
    [SerializeField] private float playAudioVolume = 1f;

    private void Start()
    {
        playButton.onClick.AddListener(Play);

        quitButton.onClick.AddListener(Quit);
    }

    private void Play()
    {
        AudioManager.Instance.PlayAudio(playClip, playAudioVolume);
        SceneManager.LoadScene(1);
    }
    private void Quit()
    {
        Application.Quit();
    }
}
