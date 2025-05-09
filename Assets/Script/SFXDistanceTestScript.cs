using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXDistanceTestScript : MonoBehaviour
{
    [SerializeField] List<AudioSource> _playingAudio;
    [SerializeField] Transform _targetTransform;
    [SerializeField] float _maxDistance;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(playSFX());
        _playingAudio.RemoveAll(a => a == null);
        if (_targetTransform == null)
            return;
        float d = Vector3.Distance(transform.position, _targetTransform.position);

        foreach(AudioSource i in _playingAudio)
        {
            i.volume = MathF.Max(0f, (_maxDistance - d) / (_maxDistance - 1));
        }
    }

    IEnumerator playSFX()
    {
        if(_playingAudio.Count == 0)
            _playingAudio.Add(SoundManager.Instance.PlaySFX("air"));
        yield return null;
    }
}
