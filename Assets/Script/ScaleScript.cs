using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScaleScript : MonoBehaviour
{
    [SerializeField] List<AudioSource> playingAudio;
    [SerializeField] string scale;
    bool _isOn = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    void CheckOverlap()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position);
        if (!hit.IsUnityNull() && hit.CompareTag("Player"))
        {
            if (!_isOn)
                playingAudio.Add(SoundManager.Instance.PlaySFX(scale, transform.position));
            _isOn = true;
        }
        else
            _isOn = false;
    }
    // Update is called once per frame
    void Update()
    {
        CheckOverlap();
    }
}
