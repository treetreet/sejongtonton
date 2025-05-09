using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Script;
using Unity.VisualScripting;
using UnityEngine;

public class CatScript : MonoBehaviour
{
    [SerializeField] List<AudioSource> _playingAudio;
    [SerializeField] Transform _targetTransform;
    [SerializeField] float _maxDistance;
    
    private Transform _playerTransform;
    void Start()
    {
        StartCoroutine(playSFX());
        _playerTransform  = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        PlayerCtrl.OnPlayerMove += CheckOverlap;
    }

    private void OnDisable()
    {
        PlayerCtrl.OnPlayerMove -= CheckOverlap;
    }

    private void CheckOverlap()
    {
        if (transform.position == _playerTransform.position)
        {
            _playingAudio.Add(SoundManager.Instance.PlaySFX("getcat", transform.position));
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        _playingAudio.RemoveAll(a => a == null);
        if (_targetTransform.IsUnityNull())
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
                int temp = UnityEngine.Random.Range(0, 2);
                string clipName = temp switch
                {
                    0 => "cat1",
                    1 => "cat2"
                };
                _playingAudio.Add(SoundManager.Instance.PlaySFX(clipName, transform.position));
            }
        }
    }
}
