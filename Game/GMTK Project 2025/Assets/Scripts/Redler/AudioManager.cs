using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayAudio(AudioClip clip, float volume = 1f, float pitch = 1, bool loop = false)
    {
        AudioSource audio = Instantiate(audioPrefab, transform);
        audio.clip = clip;
        audio.volume = volume;
        audio.pitch = pitch;
        audio.loop = loop;

        audio.Play();

        if (!loop) Destroy(audio, clip.length + 0.5f);
    }
    public void PlayAudio(AudioVariable audioVar)
    {
        AudioSource audio = Instantiate(audioPrefab, transform);
        audio.clip = audioVar.clip;
        audio.volume = audioVar.volume;
        audio.pitch = audioVar.pitch;
        audio.loop = audioVar.loop;

        audio.Play();

        if (!audioVar.loop) Destroy(audio, audioVar.clip.length + 0.5f);
    }
}

[System.Serializable]
public class AudioVariable
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(-3f, 3f)] public float pitch = 1f;
    public bool loop = false;
}
