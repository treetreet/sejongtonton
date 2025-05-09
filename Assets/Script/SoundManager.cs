using System;
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

    public AudioSource PlaySFX(string name, float volume = 1f)
    {
        if (sfxClips.TryGetValue(name, out AudioClip clip))
        {
            GameObject tempGO = new GameObject("SFX_" + name);
            AudioSource tempSource = tempGO.AddComponent<AudioSource>();
            tempSource.clip = clip;
            tempSource.volume = volume;
            tempSource.Play();

            Destroy(tempGO, clip.length); // 자동 제거
            return tempSource;
        }
        else
        {
            Debug.LogWarning($"SFX '{name}' not found.");
            return null;
        }
    }
    public AudioSource PlaySFX(string name,Vector3 pos, float volume = 1f)
    {
        if (sfxClips.TryGetValue(name, out AudioClip clip))
        {
            GameObject tempGO = new GameObject("SFX_" + name);
            tempGO.transform.position = pos;
            AudioSource tempSource = tempGO.AddComponent<AudioSource>();
            tempSource.clip = clip;
            tempSource.volume = volume;
            tempSource.spatialBlend = 1f;
            tempSource.Play();

            Destroy(tempGO, clip.length); // 자동 제거
            return tempSource;
        }
        else
        {
            Debug.LogWarning($"SFX '{name}' not found.");
            return null;
        }
    }

    public Coroutine PlaySFXCoroutine(string name)
    {

        return StartCoroutine(PlaySFXRoutine(name));
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
