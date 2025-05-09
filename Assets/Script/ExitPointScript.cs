using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPointScript : MonoBehaviour
{
    [SerializeField] List<AudioSource> playingAudio;
    [SerializeField] Transform targetTransform;
    [SerializeField] float maxDistance;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (playingAudio.Count == 0)
            playSFX();
        playingAudio.RemoveAll(a => a == null);
        if (targetTransform == null)
            return;
        float d = Vector3.Distance(transform.position, targetTransform.position);

        foreach(AudioSource i in playingAudio)
        {
            i.volume = MathF.Max(0f, (maxDistance - d) / (maxDistance - 1));
        }
    }

    void playSFX()
    {
        playingAudio.Add(SoundManager.Instance.PlaySFX("air", transform.position));
    }
}
