using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance { get; private set; }

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioSource randomPitchAudioSource;

    [SerializeField]
    private AudioSource voiceAudioSource;

    private SoundEffectLibrary soundEffectLibrary;

    [SerializeField]
    private Slider sfxSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sfxSlider.onValueChanged.AddListener(
            (_) =>
            {
                OnValueChanged();
            }
        );
    }

    public void Play(string soundName, bool isRandomPitch)
    {
        AudioClip audioClip = SoundEffectLibrary.Instance.GetRandomClip(soundName);

        if (audioClip != null)
        {
            if (randomPitchAudioSource)
            {
                randomPitchAudioSource.pitch = UnityEngine.Random.Range(1f, 5f);
                randomPitchAudioSource.PlayOneShot(audioClip);
            }
            else
                audioSource.PlayOneShot(audioClip);
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        randomPitchAudioSource.volume = volume;
        voiceAudioSource.volume = volume;
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }

    public void PlayVoice(AudioClip audioClip, float pitch = 1f)
    {
        voiceAudioSource.pitch = pitch;
        voiceAudioSource.PlayOneShot(audioClip);
    }
}
