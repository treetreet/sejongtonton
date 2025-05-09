using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LavaScript : MonoBehaviour
{
    [SerializeField] List<AudioSource> _playingAudio;
    [SerializeField] Transform _targetTransform;
    [SerializeField] float _maxDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    void CheckOverlap()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position);
        if (!hit.IsUnityNull() && hit.CompareTag("Player"))
        {
            Debug.Log("Dead");
        }
    }
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(playSFX());

        CheckOverlap();
        _playingAudio.RemoveAll(a => a == null);
        if (_targetTransform == null)
            return;
        float d = Vector3.Distance(transform.position, _targetTransform.position);

        foreach (AudioSource i in _playingAudio)
        {
            i.volume = MathF.Max(0f, (_maxDistance - d) / (_maxDistance - 1));
        }
    }

    IEnumerator playSFX()
    {
        if (_playingAudio.Count == 0)
        { 
            _playingAudio.Add(SoundManager.Instance.PlaySFX("lava_flow", transform.position));
        }
        yield return null;
    }
}
