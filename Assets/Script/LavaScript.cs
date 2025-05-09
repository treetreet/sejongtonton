using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

namespace Script
{
    public class LavaScript : MonoBehaviour
    {
        [SerializeField] List<AudioSource> playingAudio;
        [SerializeField] Transform targetTransform;
        [SerializeField] float maxDistance;

        private GameManager _gameManager;
        private Transform _playerTransform;

        void CheckOverlap()
        {
            if(transform.position == _playerTransform.position)
            {
                Debug.Log("Lava");
                _gameManager.GameOver();
            }
        }
        
        private void OnEnable()
        {
            PlayerCtrl.OnPlayerMove += CheckOverlap;
        }

        private void OnDisable()
        {
            PlayerCtrl.OnPlayerMove -= CheckOverlap;
        }

        void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        void Update()
        {
            StartCoroutine(playSFX());

            playingAudio.RemoveAll(a => a == null);
            if (targetTransform == null)
                return;
            float d = Vector3.Distance(transform.position, targetTransform.position);

            foreach (AudioSource i in playingAudio)
            {
                i.volume = MathF.Max(0f, (maxDistance - d) / (maxDistance - 1));
            }
        }

        IEnumerator playSFX()
        {
            if (playingAudio.Count == 0)
            {
                playingAudio.Add(SoundManager.Instance.PlaySFX("lava_flow", transform.position));
            }

            yield return null;
        }
    }
}
