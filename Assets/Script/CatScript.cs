using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatScript : MonoBehaviour
{
    [SerializeField] List<AudioSource> _playingAudio;
    [SerializeField] Transform _targetTransform;
    [SerializeField] float _maxDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(playSFX());
    }
    void CheckOverlap()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position);
        if (!hit.IsUnityNull() && hit.CompareTag("Player"))
        {
            _playingAudio.Add(SoundManager.Instance.PlaySFX("getcat", transform.position));
            gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {

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
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2.5f, 4f));
            Debug.Log("asdf");
            if (_playingAudio.Count == 0)
            {
                float temp = UnityEngine.Random.Range(0, 2f);
                if (temp > 1)
                    _playingAudio.Add(SoundManager.Instance.PlaySFX("cat1", transform.position));
                else
                    _playingAudio.Add(SoundManager.Instance.PlaySFX("cat2", transform.position));
            }
        }
    }
}
