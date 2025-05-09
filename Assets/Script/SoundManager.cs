using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource sfxSource;

    private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllAudio(); // 사운드 로딩
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadAllAudio()
    {
        // Resources/SFX/ 안의 모든 오디오 로드
        AudioClip[] loadedSFX = Resources.LoadAll<AudioClip>("SFX");
        foreach (var clip in loadedSFX)
        {
            sfxClips[clip.name] = clip;
        }
    }

    public void PlaySFX(string name)
    {
        if (sfxClips.TryGetValue(name, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX '{name}' not found.");
        }
    }

    public void PlaySFXCoroutine(string name)
    {

        StartCoroutine(PlaySFXRoutine(name));
    }

    private IEnumerator PlaySFXRoutine(string name)
    {
        if (sfxClips.TryGetValue(name, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
    }
}
