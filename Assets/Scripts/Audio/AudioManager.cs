using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] bgmSources;
    public AudioSource[] BGM { get { return bgmSources; } }

    [SerializeField]
    private AudioSource[] sfxSources;
    public AudioSource[] SFX { get { return sfxSources; } }

    [SerializeField]
    private AudioMixer audioMixer;

    private AudioSource currentBGM;
    public static AudioManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlayRandomBGM();
    }

    public void PlayRandomBGM()
    {
        StopAllBGM();
        int idx = Random.Range(1, bgmSources.Length);
        currentBGM = bgmSources[idx];
        currentBGM.Play();
    }

    public void PlayBGM(int index)
    {
        StopAllBGM();
        if (index >= 0 && index < bgmSources.Length)
        {
            currentBGM = bgmSources[index];
            currentBGM.Play();
        }
    }

    public void StopAllBGM()
    {
        foreach (var src in bgmSources)
            src.Stop();
    }

    // SFX ตาม index
    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxSources.Length)
            sfxSources[index].Play();
    }
}

